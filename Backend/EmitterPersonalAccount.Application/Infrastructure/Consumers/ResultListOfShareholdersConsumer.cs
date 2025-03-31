using BaseMicroservice;
using EmitterPersonalAccount.Application.Services;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Repositories;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using ExternalOrderReportsService.Consumers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Application.Infrastructure.Consumers
{
    public class ResultListOfShareholdersConsumer : BaseRabbitConsumer
    {
        private readonly IServiceProvider provider;

        public ResultListOfShareholdersConsumer(string rabbitUri, 
            IServiceProvider provider) : base(rabbitUri)
        {
            this.provider = provider;
        }
        public override async Task<Result> Handler
            (object model, BasicDeliverEventArgs args)
        {
            var orderGeneratingResult = EventDeserializer<ResultListOfShareholsders>
                .Deserialize(args);

            using (var scope = provider.CreateScope())
            {
                var resultsService = scope.ServiceProvider
                    .GetRequiredService<ResultService>();

                var memoryCacheService = scope.ServiceProvider
                    .GetRequiredService<IMemoryCacheService>();
                // Где-то тут надо сохранить это всё дело в БД
                var connectionIdFoundResult = memoryCacheService
                    .GetValue<string>(orderGeneratingResult.UserId);

                if (!connectionIdFoundResult.IsSuccessfull)
                    return connectionIdFoundResult;
                //memoryCacheService.RemoveValue(orderGeneratingResult.UserId);

                await resultsService.SendListOfShareholdersResultToClient(
                    connectionIdFoundResult.Value,
                    orderGeneratingResult.DocumentId,
                    orderGeneratingResult.SendingDate);

                var orderReportsRepository = scope.ServiceProvider
                    .GetRequiredService<IOrderReportsRepository>();

                await orderReportsRepository
                    .ChangeProcessingStatusOk(orderGeneratingResult.DocumentId,
                    orderGeneratingResult.ExternalDocumentId);
            }

            return Result.Success();
        }
    }
}
