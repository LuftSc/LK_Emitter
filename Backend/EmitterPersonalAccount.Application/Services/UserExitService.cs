
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit.Logs;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Timers;
//using System.Threading.Tasks;



namespace EmitterPersonalAccount.Application.Services
{
    public class UserExitService : IUserExitService
    {
        public UserExitService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        private static ConcurrentDictionary<Guid, System.Timers.Timer> usersLogouts = new();
        private readonly IServiceProvider serviceProvider;

        public Result OnLogout(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            var timer = new System.Timers.Timer(30000);
            timer.AutoReset = false;
            timer.Elapsed += async (e, sender) =>
            {
                await SendLogoutMessage(userId);

                usersLogouts.TryRemove(userId, out _);

                timer.Dispose();
            };

            usersLogouts[userId] = timer;
            timer.Start();

            return Result.Success();
        }
        public Result OnReload(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            usersLogouts
                .TryRemove(userId, out var timer);

            timer?.Stop();

            timer?.Dispose();

            return Result.Success();
        }
        private async Task SendLogoutMessage(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var ev = new UserActionLogEvent(
                    userId,
                    ActionLogType.LogoutOfSystem.Type,
                    // Отнимаем 30 секунд ввиду дейсвтия таймера
                    DateTime.Now.ToUniversalTime().AddHours(5).AddSeconds(-30));

                var publisher = scope.ServiceProvider
                    .GetRequiredService<IRabbitMqPublisher>();

                var deliveryResult = await publisher
                    .SendMessageAsync(
                        JsonSerializer.Serialize(ev),
                        RabbitMqAction.WriteUsersLogs,
                        cancellationToken);
            }
        }
    }
}
