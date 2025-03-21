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
            Console.WriteLine("Вызвали метод SendResult Хаба результатов");

            await Clients
                .Client(Context.ConnectionId)
                .SendListOfShareholdersResult(documentId, requestDate);
        }
        public override Task OnConnectedAsync()
        {
            var userId = Context.User.FindFirst(CustomClaims.UserId).Value;
            memoryCacheService.SetValue(userId, Context.ConnectionId);

            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User.FindFirst(CustomClaims.UserId).Value;
            memoryCacheService.RemoveValue(userId);

            return base.OnDisconnectedAsync(exception);
        }
    }
}
