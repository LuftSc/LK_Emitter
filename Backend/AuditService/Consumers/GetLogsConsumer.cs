using BaseMicroservice;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit.Logs;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.DTO;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client.Events;
using System.Text.Json;

namespace AuditService.Consumers
{
    public class GetLogsConsumer : BaseRabbitConsumer
    {
        private readonly IServiceProvider serviceProvider;

        public GetLogsConsumer
            (string rabbitUri, IServiceProvider serviceProvider) 
            : base(rabbitUri)
        {
            this.serviceProvider = serviceProvider;
        }

        public override async Task<Result> Handler(object model, BasicDeliverEventArgs args)
        {
            if (args.RoutingKey != "logs.collect")
                throw new InvalidOperationException("Неверный RoutingKey");

            var ev = EventDeserializer<GetUsersLogsEvent>
                .Deserialize(args);

            var rpcClient = serviceProvider
                .GetRequiredService<IRpcClient>();

            using(var scope = serviceProvider.CreateScope())
            {
                var auditRepository = scope.ServiceProvider
                    .GetRequiredService<IAuditRepository>();

                var excelService = scope.ServiceProvider
                    .GetRequiredService<IExcelService>();

                var publisher = scope.ServiceProvider
                    .GetRequiredService<IRabbitMqPublisher>();

                //var allFiltersEmpty

                var startDate = ev.StartDate.HasValue
                    ? ev.StartDate.Value.Date.ToUniversalTime()
                    : DateTime.MinValue.Date.ToUniversalTime();

                var endDate = ev.EndDate.HasValue 
                    ? ev.EndDate.Value.Date.AddDays(1).ToUniversalTime()
                    : DateTime.MaxValue.Date.ToUniversalTime();

                var userIdIsNull = !ev.UserId.HasValue;

                var allLogs = await auditRepository.QueryAsync(query => 
                    query
                        .AsNoTracking()
                        .Where(log => userIdIsNull || (ev.UserId.HasValue && log.UserId == ev.UserId.Value))
                        .Where(log => log.Timestamp >= startDate)
                        .Where(log => log.Timestamp < endDate)
                    , default);

                var uniqueGuids = allLogs
                    .Select(log => log.UserId)
                    .Distinct()
                    .ToList();

                var message = JsonSerializer.Serialize(uniqueGuids);

                var personalDataGettingResult = await rpcClient
                    .CallAsync<Dictionary<Guid, string>>(
                        message,
                        RabbitMqAction.GetUserPersonalInfoForLogs,
                        default);

                Console.WriteLine("Отправили запрос на расшифровку данных");

                if (!personalDataGettingResult.IsSuccessfull)
                    return Result.Error(new GettingResponseRpcServerError());

                var usersData = personalDataGettingResult.Value;
                var result = new List<ActionDTO>();

                foreach (var log in allLogs)
                {
                    var isFullNameFound = usersData.TryGetValue(log.UserId, out var fullName);

                    var name = string.Empty;
                    var surname = string.Empty;
                    var patronymic = string.Empty;

                    if (isFullNameFound)
                    {
                        var splittedFullName = fullName.Split(' ');

                        if (splittedFullName.Length == 3)
                        {
                            surname = splittedFullName[0];
                            name = splittedFullName[1];
                            patronymic = splittedFullName[2];
                        }
                        else if (splittedFullName.Length == 2)
                        {
                            surname = splittedFullName[0];
                            name = splittedFullName[1];
                        }
                        else if (splittedFullName.Length == 1)
                        {
                            name = splittedFullName[0];
                        }
                    }

                    var actionDTO = new ActionDTO
                    (
                        name,
                        surname,
                        patronymic,
                        log.ActionType,
                        log.Timestamp,
                        log.IpAddress,
                        log.AdditionalDataJSON
                    );

                    result.Add(actionDTO);
                }

                var createExcelResult = await excelService
                    .WriteLogsToExcelFile(result);

                if (!createExcelResult.IsSuccessfull)
                    return createExcelResult;

                var excelFileInBytes = createExcelResult.Value;

                var reportCreatingResult = ActionsReport
                    .Create($"Действия пользователей {DateTime.Now.ToUniversalTime().AddHours(5)}", 
                    excelFileInBytes);

                if (!reportCreatingResult.IsSuccessfull)
                    return reportCreatingResult;

                var report = reportCreatingResult.Value;

                var reportSavingToDbResult = await auditRepository
                    .SaveExcelActionsReport(report, default);

                if (!reportSavingToDbResult.IsSuccessfull)
                    return reportSavingToDbResult;

                var succesEvent = new SendResultToClientEvent()
                {
                    ContentJSON = JsonSerializer.Serialize(
                        new ActionsReportDTO(
                            ev.SenderId,
                            report.Id,
                            report.Title,
                            report.GetSize(),
                            report.DateOfGeneration)),
                    MethodForResultSending = MethodResultSending.ReceiveActionsReport
                };

                await publisher
                    .SendMessageAsync(
                        JsonSerializer.Serialize(succesEvent),
                        RabbitMqAction.SendResultToClient,
                        default);

                return Result.Success();
            }
        }
    }

    public class GettingResponseRpcServerError : Error
    {
        public override string Type => nameof(GettingResponseRpcServerError);
    }
    
}
