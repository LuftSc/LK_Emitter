using BaseMicroservice;
using EmitterPersonalAccount.Application.Services;
using EmitterPersonalAccount.Core.Abstractions;
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

                var httpContextAccessor = scope.ServiceProvider
                    .GetRequiredService<IHttpContextAccessor>();

                var userId = httpContextAccessor.HttpContext.User
                    .FindFirst(CustomClaims.UserId).Value;

                var connectionIdFoundResult = memoryCacheService.GetValue<string>(userId);

                if (!connectionIdFoundResult.IsSuccessfull)
                    return connectionIdFoundResult;

                await resultsService.SendResultToClient(
                    connectionIdFoundResult.Value,
                    orderGeneratingResult.DocumentId,
                    orderGeneratingResult.SendingDate);
            }

            return Result.Success();
        }
    }
}
