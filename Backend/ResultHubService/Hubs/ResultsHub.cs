using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Caching.Distributed;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ResultHubService.Hubs
{
    public interface IResultClient
    {
        public Task ReceiveListOfShareholdersResult(Guid documentId, string status, DateTime requestDate, Guid idForDownload);
        public Task SendReeRepResult(Guid documentId, DateTime requestDate);
        public Task SendDividendListResult(Guid documentId, DateTime requestDate);
        public Task ReceiveReports(OrderReportPaginationList orderReports);
    }
    public class ResultsHub : Hub<IResultClient>
    {
        private readonly IDistributedCache distributedCache;
        private readonly IMemoryCacheService memoryCacheService;

        //private readonly IMemoryCacheService memoryCacheService;


        public ResultsHub(IMemoryCacheService memoryCacheService)
        {
            this.distributedCache = distributedCache;
            this.memoryCacheService = memoryCacheService;
            //this.memoryCacheService = memoryCacheService;
        }
        public async Task SendResult(Guid documentId, DateTime requestDate)
        {

            /*await Clients
                .Client(Context.ConnectionId)
                .SendListOfShareholdersResult(documentId, requestDate);*/
        }
        public async Task<Result> EmitterSelected(string emitterId)
        {
            // Получить user ID

            /*var userIdResult = ClaimService.Get(Context, CustomClaims.UserId);

            if (!userIdResult.IsSuccessfull)
                return Result.Error(new UserClaimNotFoundError());*/

            // Получаем JWT из запроса (переданного Edge)
            var httpContext = Context.GetHttpContext();
            var jwtToken = httpContext.Request.Query["access_token"];
            var userId = GetUserIdFromJwt(jwtToken);

            var connInfoResult = memoryCacheService
                .GetValue<ConnectionInfo>(userId);
            //var connInfoResult = await distributedCache.GetStringAsync(userId);

            if (!connInfoResult.IsSuccessfull) return connInfoResult;

            if (connInfoResult.Value.CurrentEmitterId != string.Empty ||
                connInfoResult.Value.CurrentEmitterId == emitterId)
                await Groups.RemoveFromGroupAsync(Context.ConnectionId,
                    connInfoResult.Value.CurrentEmitterId);

            await Groups.AddToGroupAsync(Context.ConnectionId, emitterId);

            memoryCacheService.SetValue(userId,
                new ConnectionInfo(Context.ConnectionId, connInfoResult.Value.CurrentEmitterId));

            return Result.Success();
        }
        public override async Task OnConnectedAsync()
        {
            // Получить user ID
            //var userId = Context.User.FindFirst(CustomClaims.UserId).Value;

            var httpContext = Context.GetHttpContext();
            //var jwtToken = Context.Items["JwtToken"] as string;
            var jwtToken = httpContext.Request.Query["access_token"].ToString();

            var userId = GetUserIdFromJwt(jwtToken);
            //await Groups.AddToGroupAsync(Context.ConnectionId, $"user-{userId}");


            var userConnectionResult = memoryCacheService.GetValue<ConnectionInfo>(userId);
            //var userConnectionResult = await distributedCache.GetStringAsync(userId);

            /*if (userConnectionResult is null)
            {
                await distributedCache.SetStringAsync(userId, 
                    JsonSerializer.Serialize
                        (new ConnectionInfo(Context.ConnectionId, string.Empty)));
            } 
            else
            {
                var connection = JsonSerializer.Deserialize<ConnectionInfo>(userConnectionResult);

                await distributedCache.SetStringAsync(userId,
                    JsonSerializer.Serialize
                        (new ConnectionInfo(Context.ConnectionId, connection.CurrentEmitterId)));
            }*/

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

            await base.OnConnectedAsync();
            //return base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            // Получить user ID
            //var userId = Context.User.FindFirst(CustomClaims.UserId).Value;

            var httpContext = Context.GetHttpContext();
            var jwtToken = httpContext.Request.Query["access_token"].ToString();

            var userId = GetUserIdFromJwt(jwtToken);

            var userConnectionResult = memoryCacheService.GetValue<ConnectionInfo>(userId);
            //var userConnectionResult = await distributedCache.GetStringAsync(userId);

            if (userConnectionResult.IsSuccessfull)
            {
                await Groups.RemoveFromGroupAsync
                    (Context.ConnectionId, userConnectionResult.Value.CurrentEmitterId);
            }

            //memoryCacheService.RemoveValue(userId);

            await base.OnDisconnectedAsync(exception);
        }

        private string GetUserIdFromJwt(string jwtToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);
            return token.Claims.First(c => c.Type == "userId").Value;
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
