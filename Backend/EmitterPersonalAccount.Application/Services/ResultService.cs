using EmitterPersonalAccount.Application.Hubs;
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
        public ResultService(IHubContext<ResultsHub, IResultClient> hubContext)
        {
            this.hubContext = hubContext;
        }
        public async Task SendListOfShareholdersResultToClient
            (string connectionId, Guid documentId, DateTime sendingDate)
        {
            await hubContext.Clients
                .Client(connectionId)
                .SendListOfShareholdersResult(documentId, sendingDate);
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
