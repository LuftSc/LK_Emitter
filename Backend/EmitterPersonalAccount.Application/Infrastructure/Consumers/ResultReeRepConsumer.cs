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
    public class ResultReeRepConsumer : BaseRabbitConsumer
    {
        private readonly IServiceProvider provider;

        public ResultReeRepConsumer(string rabbitUri, 
            IServiceProvider provider) : base(rabbitUri)
        {
            this.provider = provider;
        }
        public override async Task<Result> Handler(object model, BasicDeliverEventArgs args)
        {
            var orderGeneratingResult = EventDeserializer<ResultReeRep>
                .Deserialize(args);

            using (var scope = provider.CreateScope())
            {
                var resultsService = scope.ServiceProvider
                    .GetRequiredService<ResultService>();

                var memoryCacheService = scope.ServiceProvider
                    .GetRequiredService<IMemoryCacheService>();

                var connectionIdFoundResult = memoryCacheService
                    .GetValue<string>(orderGeneratingResult.UserId);

                if (!connectionIdFoundResult.IsSuccessfull)
                    return connectionIdFoundResult;

                //memoryCacheService.RemoveValue(orderGeneratingResult.UserId);

                await resultsService.SendReeRepResultToClient(
                    connectionIdFoundResult.Value,
                    orderGeneratingResult.DocumentId,
                    orderGeneratingResult.SendingDate);
            }

            return Result.Success();
        }
    }
}
