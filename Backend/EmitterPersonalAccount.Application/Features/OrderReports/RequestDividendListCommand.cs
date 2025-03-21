using EmitterPersonalAccount.Application.Infrastructure.Cqs;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using ExternalOrderReportsService.Contracts;
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
        public ReportAboutDividendListNotSignRequest RequestData { get; set; }
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
            request.SendingDate = DateTime.Now;

            var message = JsonSerializer.Serialize(request);

            var isSuccesfull = await publisher.SendMessageAsync(message,
                RabbitMqAction.RequestDividendList, cancellationToken);

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
