using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit;
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
        public async Task<Result> SetProcessingStatus(string userId, OrderReport report)
        {
            var saveToDbResult = await orderReportsRepository.SaveAsync(report, default);

            if (!saveToDbResult.IsSuccessfull) return saveToDbResult;

            var eventProcessing = new SendResultToClientEvent
            {
                MethodForResultSending = "ListOfShareholders",
                ContentJSON = JsonSerializer.Serialize(new SendListOSAResultContent()
                {
                    ExternalDocumentId = report.ExternalStorageId,
                    RequestDate = report.RequestDate,
                    UserId = userId,
                    DocumentId = report.Id,
                    Status = CompletionStatus.Processing
                })
            };

            await publisher
                .SendMessageAsync(
                    JsonSerializer.Serialize(eventProcessing),
                    RabbitMqAction.SendResultToClient,
                    default);

            return Result.Success();
        }
        public async Task<Result> SetSuccessfullStatus
            (string userId, OrderReport report, Guid externalReportId)
        {
            var changeStatusDbResult = await orderReportsRepository
                .ChangeProcessingStatusOk(report.Id, externalReportId);

            if (!changeStatusDbResult.IsSuccessfull) return changeStatusDbResult;

            var eventSuccessfull = new SendResultToClientEvent
            {
                MethodForResultSending = "ListOfShareholders",
                ContentJSON = JsonSerializer.Serialize(new SendListOSAResultContent()
                {
                    ExternalDocumentId = externalReportId,
                    RequestDate = report.RequestDate,
                    UserId = userId,
                    DocumentId = report.Id,
                    Status = CompletionStatus.Successfull
                })
            };

            await publisher
                .SendMessageAsync(
                    JsonSerializer.Serialize(eventSuccessfull),
                    RabbitMqAction.SendResultToClient,
                    default);

            return Result.Success();
        }
        public async Task<Result> SetFailedStatus
            (string userId, OrderReport report)
        {
            var changeStatusDbResult = await orderReportsRepository
                .ChangeProcessingStatusFailed(report.Id);

            if (!changeStatusDbResult.IsSuccessfull) return changeStatusDbResult;

            var eventFailed = new SendResultToClientEvent
            {
                MethodForResultSending = "ListOfShareholders",
                ContentJSON = JsonSerializer.Serialize(new SendListOSAResultContent()
                {
                    ExternalDocumentId = report.ExternalStorageId,
                    RequestDate = report.RequestDate,
                    UserId = userId,
                    DocumentId = report.Id,
                    Status = CompletionStatus.Failed
                })
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
