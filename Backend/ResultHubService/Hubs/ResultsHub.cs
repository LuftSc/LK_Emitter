using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit.Documents;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit.OrderReports;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.DTO;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using ResultHubService.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ResultHubService.Hubs
{
    public interface IResultClient
    {
        public Task ReceiveReports(OrderReportPaginationList orderReports);
        public Task ReceiveReport(OrderReportDTO orderReport);
        public Task ReceiveDocuments(List<DocumentDTO> documents);
        public Task ReceiveActionsReport(ActionsReportDTO actionsReport);
    }
    public class ResultsHub : Hub<IResultClient>
    {
        private readonly IMemoryCacheService memoryCacheService;
        private readonly IDistributedCache redis;

        //private readonly IMemoryCacheService memoryCacheService;


        public ResultsHub(IMemoryCacheService memoryCacheService, IDistributedCache redis)
        {
            this.memoryCacheService = memoryCacheService;
            this.redis = redis;
            //this.memoryCacheService = memoryCacheService;
        }
        public async Task<Result> EmitterSelected(string emitterId)
        {
            var httpContext = Context.GetHttpContext();
            var jwtToken = httpContext.Request.Cookies["tasty-cookies"];
            var userId = GetUserIdFromJwt(jwtToken);
            // Получить user ID
            //var userIdResult = ClaimService.Get(Context, CustomClaims.UserId);

            //if (!userIdResult.IsSuccessfull)
               // return Result.Error(new UserClaimNotFoundError());

            // Получаем JWT из запроса (переданного Edge)
            //var httpContext = Context.GetHttpContext();
            //var jwtToken = httpContext.Request.Query["access_token"];
            //var userId = GetUserIdFromJwt(jwtToken);

            var connInfoResult = memoryCacheService
                .GetValue<ConnectionInfo>(userId);
            //var connInfoResult = await distributedCache.GetStringAsync(userId);

            if (!connInfoResult.IsSuccessfull) return connInfoResult;

            if (connInfoResult.Value.CurrentEmitterId != string.Empty ||
                connInfoResult.Value.CurrentEmitterId == emitterId)
                await Groups.RemoveFromGroupAsync(Context.ConnectionId,
                    connInfoResult.Value.CurrentEmitterId);

            Console.WriteLine($"Подключили к группе: {emitterId}");
            await Groups.AddToGroupAsync(Context.ConnectionId, emitterId);

            memoryCacheService.SetValue(userId,
                new ConnectionInfo(Context.ConnectionId, emitterId));

            return Result.Success();
        }
        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var jwtToken = httpContext.Request.Cookies["tasty-cookies"];
            var userId = GetUserIdFromJwt(jwtToken);

            //var userIdResult = ClaimService.Get(Context, CustomClaims.UserId);
            if (userId is not null)
            {
                var userConnectionResult = memoryCacheService
                    .GetValue<ConnectionInfo>(userId);

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
            }

            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var httpContext = Context.GetHttpContext();
            var jwtToken = httpContext.Request.Cookies["tasty-cookies"];
            var userId = GetUserIdFromJwt(jwtToken);

           //var userIdResult = ClaimService.Get(Context, CustomClaims.UserId);
            var userConnectionResult = memoryCacheService
                .GetValue<ConnectionInfo>(userId);

            if (userConnectionResult.IsSuccessfull)
            {
                await Groups.RemoveFromGroupAsync
                    (Context.ConnectionId, userConnectionResult.Value.CurrentEmitterId);
            }

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
