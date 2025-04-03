using EmitterPersonalAccount.Application.Infrastructure.Cqs;
using EmitterPersonalAccount.Application.Services;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.Repositories;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using EmitterPersonalAccount.DataAccess.Repositories;
using ExternalOrderReportsService.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Application.Features.OrderReports
{
    public sealed class RequestListOfShareholdersCommand : Command
    {
        public DateTime? SendingDate { get; set; }
        public ListOfShareholdersRequest RequestData { get; set; }
        public Guid EmitterId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public Guid DocumentId { get; set; } = Guid.Empty;
    }

    public sealed class RequestListOfShareholdersCommandHandler 
        : CommandHandler<RequestListOfShareholdersCommand>
    {
        private readonly IRabbitMqPublisher publisher;
        private readonly IOrderReportsRepository orderReportsRepository;
        private readonly ResultService resultService;

        public RequestListOfShareholdersCommandHandler(IRabbitMqPublisher publisher, 
            IOrderReportsRepository orderReportsRepository, 
            ResultService resultService)
        {
            this.publisher = publisher;
            this.orderReportsRepository = orderReportsRepository;
            this.resultService = resultService;
        }
        public override async Task<Result> Handle
            (RequestListOfShareholdersCommand request, 
            CancellationToken cancellationToken)
        {
            var requestDate = DateTime.Now.ToUniversalTime().AddHours(5);

            var orderReportCreateResult = OrderReport
                .Create("Лист участников собрания акционеров", requestDate);

            if (!orderReportCreateResult.IsSuccessfull)
                return orderReportCreateResult;

            request.SendingDate = requestDate;
            request.DocumentId = orderReportCreateResult.Value.Id;

            var message = JsonSerializer.Serialize(request);

            var isSuccesfull = await publisher.SendMessageAsync(message, 
                RabbitMqAction.RequestListOfShareholders, cancellationToken);

            if (!isSuccesfull) return Result
                    .Error(new SendingListOfShareholdersRequestError());

            var saveResult = await orderReportsRepository
                .SaveAsync(request.EmitterId, orderReportCreateResult.Value);

            if (!saveResult.IsSuccessfull)
            {
                await resultService.SendListOfShareholdersResultToClient(
                        Guid.Empty,
                        requestDate,
                        request.UserId,
                        request.DocumentId,
                        ReportOrderStatus.Failed);

                return saveResult;
            }

            await resultService.SendListOfShareholdersResultToClient(
                        Guid.Empty,
                        requestDate,
                        request.UserId,
                        request.DocumentId,
                        ReportOrderStatus.Processing);

            return Result.Success();
        }
    }
    public class SendingListOfShareholdersRequestError : Error
    {
        public override string Type => nameof(SendingListOfShareholdersRequestError);
    }
}
