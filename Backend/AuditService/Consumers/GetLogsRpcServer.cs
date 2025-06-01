using BaseMicroservice;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit.Logs;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using System.Text.Json;

namespace AuditService.Consumers
{
    //public class GetLogsRpcServer : RpcServerBase<Result<List<UserActionLogEvent>>>
    //{
    //    private readonly IServiceProvider provider;

    //    public GetLogsRpcServer(
    //        string rabbitUri, 
    //        IServiceProvider provider) : base(rabbitUri)
    //    {
    //        this.provider = provider;
    //    }

    //    public override async Task<Result<List<UserActionLogEvent>>> OnMessageProcessingAsync(string message)
    //    {
    //        var filters = JsonSerializer
    //            .Deserialize<GetUsersLogsEvent>(message);

    //        using(var scope = provider.CreateScope())
    //        {
    //            var auditRepository = scope.ServiceProvider
    //                .GetRequiredService<IAuditRepository>();

    //            var logsEntities = await auditRepository.ListAsync(default);

    //            var logs = logsEntities
    //                .Select(l => new UserActionLogEvent(
    //                    l.UserId,
    //                    l.ActionType,
    //                    l.Timestamp,
    //                    l.IpAddress,
    //                    l.AdditionalDataJSON
    //                    ))
    //                .ToList();

    //            return Result<List<UserActionLogEvent>>.Success(logs); 
    //        }
    //    }

    //    public override Task<Result<List<UserActionLogEvent>>> OnMessageProcessingFailureAsync(Exception exception)
    //    {
    //        return Task.FromResult(Result<List<UserActionLogEvent>>
    //            .Error(new ActionCollectError()));
    //    }
    //}

    //public class ActionCollectError : Error
    //{
    //    public override string Type => nameof(ActionCollectError);
    //}
}
