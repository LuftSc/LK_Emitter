using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit.Documents;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit.OrderReports;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit.OrderReports.ListOfShareholders;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.DTO;
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
        public async Task SendReportToClient(OrderReportDTO orderReport)
        {
            var connInfoResult = memoryCache.GetValue<Hubs.ConnectionInfo>(orderReport.UserId);

            if (connInfoResult.IsSuccessfull)
            {
                await hubContext.Clients
                    .Group(connInfoResult.Value.CurrentEmitterId)
                    .ReceiveReport(orderReport);
            }
        }
        public async Task SendDocumentsToClient(DocumentsContent content)
        {
            var connInfoResult = memoryCache.GetValue<Hubs.ConnectionInfo>(content.UserId);

            if (connInfoResult.IsSuccessfull)
            {
                await hubContext.Clients
                    .Group(connInfoResult.Value.CurrentEmitterId)
                    .ReceiveDocuments(content.Documents);
            }
        }
        public async Task SendActionsReportToClient(ActionsReportDTO report)
        {
            var connInfoResult = memoryCache.GetValue<Hubs.ConnectionInfo>(report.SenderId);

            if (connInfoResult.IsSuccessfull)
            {
                await hubContext.Clients
                    .Client(connInfoResult.Value.ConnectionId)
                    .ReceiveActionsReport(report);
            }
        }
    }
}
