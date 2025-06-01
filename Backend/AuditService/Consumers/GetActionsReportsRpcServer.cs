using BaseMicroservice;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.SharedKernal.DTO;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using RabbitMQ.Client.Events;
using System.Text.Json;

namespace AuditService.Consumers
{
    public class GetActionsReportsRpcServer : RpcServerBase<Result<List<ActionsReportDTO>>>
    {
        private readonly IServiceProvider serviceProvider;

        public GetActionsReportsRpcServer
            (string rabbitUri, IServiceProvider serviceProvider) : base(rabbitUri)
        {
            this.serviceProvider = serviceProvider;
        }

        public override async Task<Result<List<ActionsReportDTO>>> OnMessageProcessingAsync
            (string message, BasicDeliverEventArgs args)
        {
            if (args.RoutingKey != "logs.get")
                throw new InvalidOperationException("Неверный RoutingKey");

            var ev = JsonSerializer
                .Deserialize<DateTime>(message);
            Console.WriteLine("Получили сообщение о получении списка отчётов");

            using (var scope = serviceProvider.CreateScope())
            {
                var auditRepository = scope.ServiceProvider
                    .GetRequiredService<IAuditRepository>();

                var gettingReportsResult = await auditRepository
                    .GetActionsReports(default);

                if (!gettingReportsResult.IsSuccessfull)
                    return Result<List<ActionsReportDTO>>
                        .Error(new GettingActionsReportsError());

                var reports = gettingReportsResult.Value
                    .Select(report => new ActionsReportDTO(
                        "", 
                        report.Id, 
                        report.Title, 
                        report.GetSize(), 
                        report.DateOfGeneration))
                    .ToList();

                return Result<List<ActionsReportDTO>>
                    .Success(reports);
            }
        }
    }

    public class GettingActionsReportsError : Error
    {
        public override string Type => nameof(GettingActionsReportsError);
    }
}
