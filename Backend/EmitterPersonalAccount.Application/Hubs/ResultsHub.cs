using EmitterPersonalAccount.Application.Services;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Application.Hubs
{
    public interface IResultClient
    {
        public Task ReceiveListOfShareholdersResult(Guid documentId, string status, DateTime requestDate, Guid idForDownload);
        public Task SendReeRepResult(Guid documentId, DateTime requestDate);
        public Task SendDividendListResult(Guid documentId, DateTime requestDate);
    }
    public class ResultsHub : Hub<IResultClient>
    {
        private readonly IMemoryCacheService memoryCacheService;

        public ResultsHub(IMemoryCacheService memoryCacheService)
        {
            this.memoryCacheService = memoryCacheService;
        }
        public async Task SendResult(Guid documentId, DateTime requestDate)
        {

            /*await Clients
                .Client(Context.ConnectionId)
                .SendListOfShareholdersResult(documentId, requestDate);*/
        }
        public async Task<Result> EmitterSelected(string emitterId)
        {
            var userIdResult = ClaimService.Get(Context, CustomClaims.UserId);

            if (!userIdResult.IsSuccessfull)
                return Result.Error(new UserClaimNotFoundError());

            if (string.IsNullOrEmpty(emitterId))
                return Result.Error(new SelectedEmitterIdIsEmptyError());

            var connInfoResult = memoryCacheService
                .GetValue<ConnectionInfo>(userIdResult.Value);

            if (!connInfoResult.IsSuccessfull) return connInfoResult;

            if (connInfoResult.Value.CurrentEmitterId != string.Empty || 
                connInfoResult.Value.CurrentEmitterId == emitterId)
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, 
                    connInfoResult.Value.CurrentEmitterId);

            await Groups.AddToGroupAsync(Context.ConnectionId, emitterId);

            memoryCacheService.SetValue(userIdResult.Value, 
                new ConnectionInfo(Context.ConnectionId, emitterId));

            return Result.Success();
        }
        public override Task OnConnectedAsync()
        {
            var userId = Context.User.FindFirst(CustomClaims.UserId).Value;

            var userConnectionResult = memoryCacheService.GetValue<ConnectionInfo>(userId); 

            if (!userConnectionResult.IsSuccessfull)
            {
                memoryCacheService.SetValue(userId,
                    new ConnectionInfo(Context.ConnectionId, string.Empty));
            } 
            else
            {
                memoryCacheService.SetValue(userId, 
                    new ConnectionInfo(Context.ConnectionId, 
                    userConnectionResult.Value.CurrentEmitterId));
            }

            return base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User.FindFirst(CustomClaims.UserId).Value;
            var userConnectionResult = memoryCacheService.GetValue<ConnectionInfo>(userId);
            
            if (userConnectionResult.IsSuccessfull)
            {
                await Groups.RemoveFromGroupAsync
                    (Context.ConnectionId, userConnectionResult.Value.CurrentEmitterId);
            }
            
            memoryCacheService.RemoveValue(userId);

            await base.OnDisconnectedAsync(exception);
        }
    }

    public class SelectedEmitterIdIsEmptyError : Error
    {
        public override string Type => nameof(SelectedEmitterIdIsEmptyError);
    }
    public record ConnectionInfo(
        string ConnectionId,
        string CurrentEmitterId
        )
    { }
}
