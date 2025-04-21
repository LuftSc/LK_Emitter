using EmitterPersonalAccount.Application.Infrastructure.Cqs;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit.OrderReports;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit.OrderReports.ReeRepNotSign;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.DTO;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
//using ExternalOrderReportsService.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Application.Features.OrderReports
{
    public sealed class RequestReeRepCommand : Command
    {
        public DateTime? SendingDate { get; set; }
        public ReeRepNotSignReportData RequestData { get; set; }
        public string UserId { get; set; } = string.Empty;
    }
    public class RequestReeRepCommandHandler : CommandHandler<RequestReeRepCommand>
    {
        private readonly IRabbitMqPublisher publisher;

        public RequestReeRepCommandHandler(IRabbitMqPublisher publisher)
        {
            this.publisher = publisher;
        }
        public override async Task<Result> Handle
            (RequestReeRepCommand request, CancellationToken cancellationToken)
        {
            var requestDate = DateTime.Now.ToUniversalTime().AddHours(5);

            request.SendingDate = requestDate;

            //var ev = new RequestReeRepNotSignEvent(requestDate, request.RequestData, request.UserId);
            var ev = new RequestOrderReportEvent()
            {
                ReportType = ReportType.ReeRepNotSign,
                SendingDate = requestDate,
                UserId = request.UserId,
                RequestDataJSON = JsonSerializer.Serialize(request.RequestData)
            };

            var message = JsonSerializer.Serialize(ev);

            var isSuccesfull = await publisher.SendMessageAsync(message,
                RabbitMqAction.RequestOrderReport, cancellationToken);

            if (!isSuccesfull) return Result
                    .Error(new SendingReeRepRequestError());

            return Result.Success();
        }
    }
    public class SendingReeRepRequestError : Error
    {
        public override string Type => nameof(SendingReeRepRequestError);
    }
}
