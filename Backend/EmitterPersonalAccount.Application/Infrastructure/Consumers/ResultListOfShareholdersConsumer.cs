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
                var orderReportsRepository = scope.ServiceProvider
                    .GetRequiredService<IOrderReportsRepository>();

                var resultsService = scope.ServiceProvider
                    .GetRequiredService<ResultService>();

                var changeResult = await orderReportsRepository
                    .ChangeProcessingStatusOk(orderGeneratingResult.DocumentId,
                    orderGeneratingResult.ExternalDocumentId);

                if (!changeResult.IsSuccessfull)
                {
                    await resultsService.SendListOfShareholdersResultToClient(
                        orderGeneratingResult.ExternalDocumentId,
                        orderGeneratingResult.SendingDate,
                        orderGeneratingResult.UserId,
                        orderGeneratingResult.DocumentId,
                        ReportOrderStatus.Failed);

                    return changeResult;
                }

                await resultsService.SendListOfShareholdersResultToClient(
                    orderGeneratingResult.ExternalDocumentId,
                    orderGeneratingResult.SendingDate,
                    orderGeneratingResult.UserId,
                    orderGeneratingResult.DocumentId,
                    ReportOrderStatus.Successfull
                );
            }

            return Result.Success();
        }
    }
}
