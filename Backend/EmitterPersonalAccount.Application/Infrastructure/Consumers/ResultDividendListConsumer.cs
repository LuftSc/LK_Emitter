using BaseMicroservice;
using EmitterPersonalAccount.Application.Services;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using ExternalOrderReportsService.Consumers;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Application.Infrastructure.Consumers
{
    public class ResultDividendListConsumer : BaseRabbitConsumer
    {
        private readonly IServiceProvider provider;

        public ResultDividendListConsumer(string rabbitUri, 
            IServiceProvider provider) : base(rabbitUri)
        {
            this.provider = provider;
        }
        public override async Task<Result> Handler(object model, BasicDeliverEventArgs args)
        {
            var orderGeneratingResult = EventDeserializer<ResultDividendList>
                .Deserialize(args);

            using (var scope = provider.CreateScope())
            {
                var resultsService = scope.ServiceProvider
                    .GetRequiredService<ResultService>();

                var memoryCacheService = scope.ServiceProvider
                    .GetRequiredService<IMemoryCacheService>();

                var connectionIdFoundResult = memoryCacheService
                    .GetValue<string>(orderGeneratingResult.UserId);
                // Где-то тут надо сохранить это всё дело в БД
                if (!connectionIdFoundResult.IsSuccessfull)
                    return connectionIdFoundResult;

               // memoryCacheService.RemoveValue(orderGeneratingResult.UserId);

                await resultsService.SendDividendListResultToClient(
                    connectionIdFoundResult.Value,
                    orderGeneratingResult.DocumentId,
                    orderGeneratingResult.SendingDate);
            }

            return Result.Success();
        }
    }
}
