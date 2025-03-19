using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
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
        public Task SendListOfShareholdersResult(Guid documentId, DateTime requestDate);
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
            await Clients
                .Client(Context.ConnectionId)
                .SendListOfShareholdersResult(documentId, requestDate);
        }
        public override Task OnConnectedAsync()
        {
            try
            {
                if (Context.User == null)
                {
                    Console.WriteLine("Context.User is null.");
                    throw new InvalidOperationException("User is not authenticated.");
                }

                // Логирование всех клэймов
                foreach (var claim in Context.User.Claims)
                {
                    Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
                }

                var userIdClaim = Context.User.FindFirst(CustomClaims.UserId);
                if (userIdClaim == null)
                {
                    Console.WriteLine("User ID claim is null.");
                    throw new InvalidOperationException("User ID claim not found.");
                }

                var userId = userIdClaim.Value;
                Console.WriteLine($"User {userId} connected.");
                return base.OnConnectedAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in OnConnectedAsync: {ex}");
                throw;
            }
            /*var userId = Context.User.FindFirst(CustomClaims.UserId).Value;
            memoryCacheService.SetValue(userId, Context.ConnectionId);*/

            //return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User.FindFirst(CustomClaims.UserId).Value;
            memoryCacheService.RemoveValue(userId);

            return base.OnDisconnectedAsync(exception);
        }
    }
}
