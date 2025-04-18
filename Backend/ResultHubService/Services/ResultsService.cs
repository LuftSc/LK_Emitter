using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit.OrderReports;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit.OrderReports.ListOfShareholders;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using ResultHubService.Hubs;
using StackExchange.Redis;

namespace ResultHubService.Services
{
    public class ResultService
    {
        private readonly IHubContext<ResultsHub, IResultClient> hubContext;
        private readonly IMemoryCacheService memoryCache;
        private readonly IDistributedCache redis;

        public ResultService(IHubContext<ResultsHub, IResultClient> hubContext,
            IMemoryCacheService memoryCache, IDistributedCache redis)
        {
            this.hubContext = hubContext;
            this.memoryCache = memoryCache;
            this.redis = redis;
        }
        public async Task SendReportsToClient(ReportsPaginationListContent content)
        {
            var connInfoResult = memoryCache.GetValue<Hubs.ConnectionInfo>(content.UserId);


            if (connInfoResult.IsSuccessfull)
            {
                await hubContext.Clients
                    .Client(connInfoResult.Value.ConnectionId)
                    .ReceiveReports(content.OrderReports);
            }
        }
        public async Task SendListOfShareholdersResultToClient
            (OrderReportDTO content)
        {
            var connInfoResult = memoryCache.GetValue<Hubs.ConnectionInfo>(content.UserId);

            if (connInfoResult.IsSuccessfull)
            {
                await hubContext.Clients
                    .Group(connInfoResult.Value.CurrentEmitterId)
                    .ReceiveListOfShareholdersResult
                        (content.InternalId, content.Status.ToString(), content.RequestDate, content.IdForDownload);
            }
        }
        public async Task SendReeRepResultToClient
            (OrderReportDTO content)
        {
            var connInfoResult = memoryCache.GetValue<Hubs.ConnectionInfo>(content.UserId);

            if (connInfoResult.IsSuccessfull)
            {
                await hubContext.Clients
                    .Group(connInfoResult.Value.CurrentEmitterId)
                    .ReceiveListOfShareholdersResult
                        (content.InternalId, content.Status.ToString(), content.RequestDate, content.IdForDownload);
            }
        }
        public async Task SendDividendListResultToClient
            (OrderReportDTO content)
        {
            var connInfoResult = memoryCache.GetValue<Hubs.ConnectionInfo>(content.UserId);

            if (connInfoResult.IsSuccessfull)
            {
                await hubContext.Clients
                    .Group(connInfoResult.Value.CurrentEmitterId)
                    .ReceiveListOfShareholdersResult
                        (content.InternalId, content.Status.ToString(), content.RequestDate, content.IdForDownload);
            }
        }
    }
}
