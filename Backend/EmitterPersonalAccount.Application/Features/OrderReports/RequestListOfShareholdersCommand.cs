using EmitterPersonalAccount.Application.Infrastructure.Cqs;
using EmitterPersonalAccount.Application.Services;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit.OrderReports;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit.OrderReports.ListOfShareholders;
using EmitterPersonalAccount.Core.Domain.Repositories;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.DTO;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using EmitterPersonalAccount.DataAccess.Repositories;
//using ExternalOrderReportsService.Contracts;
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
        //public ListOfShareholdersReportData RequestData { get; set; }
        public GenerateListOSARequest RequestData { get; set; }
        public string UserId { get; set; } = string.Empty;
        //public Guid DocumentId { get; set; } = Guid.Empty;
    }

    public sealed class RequestListOfShareholdersCommandHandler 
        : CommandHandler<RequestListOfShareholdersCommand>
    {
        private readonly IRabbitMqPublisher publisher;

        public RequestListOfShareholdersCommandHandler(IRabbitMqPublisher publisher)
        {
            this.publisher = publisher;
        }
        public override async Task<Result> Handle
            (RequestListOfShareholdersCommand request, 
            CancellationToken cancellationToken)
        {
            var requestDate = DateTime.Now.ToUniversalTime().AddHours(5);

            request.SendingDate = requestDate;

            //var ev = new RequestListOfShareholdersEvent(requestDate, request.RequestData, request.UserId);
            var ev = new RequestOrderReportEvent()
            {
                ReportType = ReportType.ListOfShareholders,
                SendingDate = requestDate,
                UserId = request.UserId,
                RequestDataJSON = JsonSerializer.Serialize(request.RequestData)
            };

            var message = JsonSerializer.Serialize(ev);

            var isSuccesfull = await publisher.SendMessageAsync(message, 
                RabbitMqAction.RequestOrderReport, cancellationToken);

            if (!isSuccesfull) return Result
                    .Error(new SendingListOfShareholdersRequestError());

            return Result.Success();
        }
    }
    public class SendingListOfShareholdersRequestError : Error
    {
        public override string Type => nameof(SendingListOfShareholdersRequestError);
    }
}
