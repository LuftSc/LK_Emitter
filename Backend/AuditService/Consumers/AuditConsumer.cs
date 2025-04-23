using BaseMicroservice;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit.Logs;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using RabbitMQ.Client.Events;

namespace AuditService.Consumers
{
    public class AuditConsumer : BaseRabbitConsumer
    {
        private readonly IServiceProvider provider;
        private readonly IAuditLogService auditLogService;

        public AuditConsumer(string rabbitUri, 
            IServiceProvider provider) : base(rabbitUri)
        {
            this.provider = provider;
            auditLogService = provider.GetRequiredService<IAuditLogService>();
        }
        public override async Task<Result> Handler(object model, BasicDeliverEventArgs args)
        {
            var ev = EventDeserializer<UserActionLogEvent>
                .Deserialize(args);

            var logCreatingResult = UserActionLog.Create
                (Guid.Parse(ev.UserId), ev.Type, ev.IpAddress, ev.AdditionalDataJSON);

            if (logCreatingResult.IsSuccessfull)
                await auditLogService.AddLogAsync(logCreatingResult.Value);

            return Result.Success();
        }
    }
}
