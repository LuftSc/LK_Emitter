using BaseMicroservice;
using EmitterPersonalAccount.Application.Services;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Application.Infrastructure.Consumers
{
    public class DecryptPersonalDataRpcServer : RpcServerBase<Result<Dictionary<Guid, string>>>
    {
        private readonly IServiceProvider serviceProvider;

        public DecryptPersonalDataRpcServer
            (string rabbitUri, IServiceProvider serviceProvider) : base(rabbitUri)
        {
            this.serviceProvider = serviceProvider;
        }

        public override async Task<Result<Dictionary<Guid, string>>> 
            OnMessageProcessingAsync(string message, BasicDeliverEventArgs args)
        {
            if (args.RoutingKey != "logs.decrypt")
                throw new InvalidOperationException("Неверный RoutingKey");

            var listGuids = JsonSerializer
                .Deserialize<List<Guid>>(message);

            using (var scope = serviceProvider.CreateScope())
            {
                var usersService = scope.ServiceProvider
                    .GetRequiredService<IUsersService>();

                var fullNamesGettingResult = await usersService
                    .GetListUsesFullName(listGuids, default);

                if (!fullNamesGettingResult.IsSuccessfull)
                    return fullNamesGettingResult;

                return Result<Dictionary<Guid, string>>
                    .Success(fullNamesGettingResult.Value);
            }
        }
    }

    public class DecryptUserPErsonalDataError : Error
    {
        public override string Type => nameof(DecryptUserPErsonalDataError);
    }
}
