using EmitterPersonalAccount.Application.Infrastructure.Cqs;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit.OrderReports;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit.OrderReports.DividendList;
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
    public sealed class RequestDividendListCommand : Command
    {
        public DateTime? SendingDate { get; set; }
        public GenerateDividendListRequest RequestData { get; set; }
        public string UserId { get; set; } = string.Empty;
    }
    public sealed class RequestDividendListCommandHandler
        : CommandHandler<RequestDividendListCommand>
    {
        private readonly IRabbitMqPublisher publisher;

        public RequestDividendListCommandHandler(IRabbitMqPublisher publisher)
        {
            this.publisher = publisher;
        }
        public override async Task<Result> Handle
            (RequestDividendListCommand request, 
            CancellationToken cancellationToken)
        {
            var requestDate = DateTime.Now.ToUniversalTime().AddHours(5);

            request.SendingDate = requestDate;

            //var ev = new RequestDividendListEvent(requestDate, request.RequestData, request.UserId);
            var ev = new RequestOrderReportEvent()
            {
                ReportType = ReportType.DividendList,
                SendingDate = requestDate,
                UserId = request.UserId,
                RequestDataJSON = JsonSerializer.Serialize(request.RequestData)
            };

            var message = JsonSerializer.Serialize(ev);

            var isSuccesfull = await publisher.SendMessageAsync(message,
                RabbitMqAction.RequestOrderReport, cancellationToken);

            if (!isSuccesfull) return Result
                    .Error(new SendingDividendListRequestError());

            return Result.Success();
        }
    }
    public class SendingDividendListRequestError : Error
    {
        public override string Type => nameof(SendingDividendListRequestError);
    }
}
