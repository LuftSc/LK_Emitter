using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit.OrderReports;
using EmitterPersonalAccount.Core.Domain.Repositories;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using System.Text.Json;

namespace ExternalOrderReportsService.Services
{
    public class ReportStatusChangeService : IReportStatusChangeService
    {
        private readonly IOrderReportsRepository orderReportsRepository;
        private readonly IRabbitMqPublisher publisher;

        public ReportStatusChangeService(IOrderReportsRepository orderReportsRepository,
            IRabbitMqPublisher publisher)
        {
            this.orderReportsRepository = orderReportsRepository;
            this.publisher = publisher;
        }
        public async Task<Result> SetProcessingStatus(string userId, OrderReport report, MethodResultSending method)
        {
            var saveToDbResult = await orderReportsRepository.SaveAsync(report, default);

            if (!saveToDbResult.IsSuccessfull) return saveToDbResult;

            var eventProcessing = new SendResultToClientEvent
            {
                MethodForResultSending = method,
                ContentJSON = JsonSerializer.Serialize(new OrderReportDTO(
                    report.ExternalStorageId,
                    report.Id,
                    report.FileName,
                    CompletionStatus.Processing,
                    report.RequestDate,
                    userId
                    ))
            };

            await publisher
                .SendMessageAsync(
                    JsonSerializer.Serialize(eventProcessing),
                    RabbitMqAction.SendResultToClient,
                    default);

            return Result.Success();
        }
        public async Task<Result> SetSuccessfullStatus
            (string userId, OrderReport report, Guid externalReportId, MethodResultSending method)
        {
            var changeStatusDbResult = await orderReportsRepository
                .ChangeProcessingStatusOk(report.Id, externalReportId);

            if (!changeStatusDbResult.IsSuccessfull) return changeStatusDbResult;

            var eventSuccessfull = new SendResultToClientEvent
            {
                MethodForResultSending = method,
                ContentJSON = JsonSerializer.Serialize(new OrderReportDTO(
                    externalReportId,
                    report.Id,
                    report.FileName,
                    CompletionStatus.Successfull,
                    report.RequestDate,
                    userId
                    ))
            };

            await publisher
                .SendMessageAsync(
                    JsonSerializer.Serialize(eventSuccessfull),
                    RabbitMqAction.SendResultToClient,
                    default);

            return Result.Success();
        }
        public async Task<Result> SetFailedStatus
            (string userId, OrderReport report, MethodResultSending method)
        {
            var changeStatusDbResult = await orderReportsRepository
                .ChangeProcessingStatusFailed(report.Id);

            if (!changeStatusDbResult.IsSuccessfull) return changeStatusDbResult;

            var eventFailed = new SendResultToClientEvent
            {
                MethodForResultSending = method,
                ContentJSON = JsonSerializer.Serialize(new OrderReportDTO(
                    report.ExternalStorageId,
                    report.Id,
                    report.FileName,
                    CompletionStatus.Failed,
                    report.RequestDate,
                    userId
                ))
                
            };

            await publisher
                .SendMessageAsync(
                    JsonSerializer.Serialize(eventFailed),
                    RabbitMqAction.SendResultToClient,
                    default);

            return Result.Success();
        }
    }
}
