/*using EmitterPersonalAccount.Application.Hubs;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Application.Services
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
            (Guid externalDocumentId,
            DateTime requestDate,
            string userId,
            Guid documentId,
            CompletionStatus status)
        {
            var connInfoResult = memoryCache.GetValue<ConnectionInfo>(userId);

            if (connInfoResult.IsSuccessfull)
            {
                await hubContext.Clients
                    .Group(connInfoResult.Value.CurrentEmitterId)
                    .ReceiveListOfShareholdersResult
                        (documentId, status.ToString(), requestDate, externalDocumentId);
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
*/