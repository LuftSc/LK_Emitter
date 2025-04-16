using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using Microsoft.AspNetCore.SignalR;
using ResultHubService.Hubs;

namespace ResultHubService.Services
{
    public class ResultService
    {
        private readonly IHubContext<ResultsHub, IResultClient> hubContext;
        private readonly IMemoryCacheService memoryCache;

        public ResultService(IHubContext<ResultsHub, IResultClient> hubContext,
            IMemoryCacheService memoryCache)
        {
            this.hubContext = hubContext;
            this.memoryCache = memoryCache;
        }
        public async Task SendListOfShareholdersResultToClient
            (SendListOSAResultContent content)
        {
            var connInfoResult = memoryCache.GetValue<Hubs.ConnectionInfo>(content.UserId);

            if (connInfoResult.IsSuccessfull)
            {
                await hubContext.Clients
                    .Group(connInfoResult.Value.CurrentEmitterId)
                    .ReceiveListOfShareholdersResult
                        (content.DocumentId, content.Status.ToString(), content.RequestDate, content.ExternalDocumentId);
            }
        }
        public async Task SendReportsToClient(ReportsPaginationListContent content)
        {
            var connInfoResult = memoryCache.GetValue<Hubs.ConnectionInfo>(content.UserId);
            
            if (connInfoResult.IsSuccessfull)
            {
                Console.WriteLine($"Отправляем отчёты коннекшену: {connInfoResult.Value.ConnectionId}");
                await hubContext.Clients
                    .Client(connInfoResult.Value.ConnectionId)
                    .ReceiveReports(content.OrderReports);
            }
        }
        public async Task SendReeRepResultToClient
            (string connectionId, Guid documentId, DateTime sendingDate)
        {
            await hubContext.Clients
                .Client(connectionId)
                .SendReeRepResult(documentId, sendingDate);
        }
        public async Task SendDividendListResultToClient
            (string connectionId, Guid documentId, DateTime sendingDate)
        {
            await hubContext.Clients
                .Client(connectionId)
                .SendDividendListResult(documentId, sendingDate);
        }
    }
}
