using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit;
using EmitterPersonalAccount.Core.Domain.Repositories;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Application.Services
{
    public class OutboxPublisherService : BackgroundService
    {
        private readonly IServiceProvider provider;

        public OutboxPublisherService(IServiceProvider provider)
        {
            this.provider = provider;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested) 
            {
                using (var scope = provider.CreateScope())
                {
                    var outboxMessageRepository = scope.ServiceProvider
                        .GetRequiredService<IOutboxMessagesRepository>();

                    var newMessages = await outboxMessageRepository
                        .GetNewlyAdded(stoppingToken);

                    if (newMessages.IsSuccessfull && newMessages.Value.Count > 0)
                    {
                        foreach (var msg in newMessages.Value)
                        {
                            var publisher = scope.ServiceProvider
                                .GetRequiredService<IRabbitMqPublisher>();

                            var message = new SendOutboxMessageEvent(msg.Type, msg.ContentJSON);

                            var deliveryResult = await publisher.SendMessageAsync(
                                JsonSerializer.Serialize(message), RabbitMqAction.SendOutboxMessage, stoppingToken);

                            if (deliveryResult)
                            {
                                await outboxMessageRepository.SetStatusProcessed(msg.Id, stoppingToken);
                            }
                            else
                            {
                                await outboxMessageRepository
                                    .SetStatusFailed(msg.Id, $"Delivery to outbox Failed", stoppingToken);
                            }
                        }
                    }

                    await Task.Delay(5000, stoppingToken);
                }
            }
        }
    }
}
