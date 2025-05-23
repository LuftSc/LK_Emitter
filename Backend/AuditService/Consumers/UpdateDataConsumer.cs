using AuditService.DataAccess.Repositories;
using BaseMicroservice;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.Models.Postgres.EmitterModel.EmitterVO;
using EmitterPersonalAccount.Core.Domain.Models.Postgres.PartialModels;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit;
using EmitterPersonalAccount.Core.Domain.Repositories;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using RabbitMQ.Client.Events;
using System.Text.Json;
using IEmittersRepository = AuditService.DataAccess.Repositories.IEmittersRepository;

namespace AuditService.Consumers
{
    public class UpdateDataConsumer : BaseRabbitConsumer
    {
        private readonly IServiceProvider provider;

        public UpdateDataConsumer(string rabbitUri, IServiceProvider provider) 
            : base(rabbitUri)
        {
            this.provider = provider;
        }

        public override async Task<Result> Handler(object model, BasicDeliverEventArgs args)
        {
            var outboxMessage = EventDeserializer<SendOutboxMessageEvent>
                .Deserialize(args);

            using (var scope = provider.CreateScope())
            {
                var userRepository = scope.ServiceProvider
                            .GetRequiredService<IUsersRepository>();

                var emitterRepository = scope.ServiceProvider
                    .GetRequiredService<IEmittersRepository>();

                switch (outboxMessage.MessageType)
                {
                    case OutboxMessageType.AddUser:
                        var userData = JsonSerializer
                            .Deserialize<Tuple<Guid, List<Guid>, string>>(outboxMessage.ContentJSON);

                        var userProjection = UserProjection.Create(userData.Item1, userData.Item3);

                        return await userRepository
                            .AddUserWithEmittersBindings(userProjection.Value, userData.Item2, default);

                    case OutboxMessageType.AddUserEmitterBinding:

                        var bindingData = JsonSerializer
                            .Deserialize<Tuple<List<Guid>, Guid>>(outboxMessage.ContentJSON);

                        return await userRepository
                            .BindToEmitters(bindingData.Item1, bindingData.Item2, default);

                    case OutboxMessageType.UpdateUser:
                        return await Task.FromResult(Result.Success());
                        break;
                    case OutboxMessageType.AddEmitter:

                        var emitterData = JsonSerializer
                            .Deserialize<Tuple<Guid, EmitterInfo, int>>(outboxMessage.ContentJSON);

                        var emitterCreatingResult = EmitterProjection
                            .Create(emitterData.Item1, emitterData.Item2, emitterData.Item3);

                        await emitterRepository.AddAsync(emitterCreatingResult.Value, default);
                        await emitterRepository.UnitOfWork.SaveChangesAsync(default);

                        return Result.Success();

                        break;
                    case OutboxMessageType.UpdateEmitter:
                        return await Task.FromResult(Result.Success());
                        break;
                    default:
                        return await Task.FromResult(Result.Error(new OutboxMessageTypeUnsupportedError()));
                }
            }

               
        }
        public class OutboxMessageTypeUnsupportedError : Error
        {
            public override string Type => nameof(OutboxMessageTypeUnsupportedError);
        }
    }
}
