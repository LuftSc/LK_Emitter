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
    public sealed class RequestListOfShareholdersCommand : Command
    {
        public DateTime? SendingDate { get; set; }
        public ListOfShareholdersRequest RequestData { get; set; }
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
            request.SendingDate = DateTime.Now;

            var message = JsonSerializer.Serialize(request);

            var isSuccesfull = await publisher.SendMessageAsync(message, 
                RabbitMqAction.RequestListOfShareholders, cancellationToken);

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
