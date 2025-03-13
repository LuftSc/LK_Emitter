using EmitterPersonalAccount.Application.Infrastructure.Cqs;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Caching.Distributed;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Cms;
using System.Security.Cryptography;
using System.Text.Json;

namespace AuthService.Application.Features.Email
{
    public sealed class SendConfirmationEmailCommand : Command
    {
        public string Recipient { get; set; }
    }

    public sealed class SendConfirmationEmailCommandHandler
        : CommandHandler<SendConfirmationEmailCommand>
    {
        private readonly IDistributedCache cache;
        private readonly IRabbitMqPublisher publisher;

        public SendConfirmationEmailCommandHandler(IDistributedCache cache, IRabbitMqPublisher publisher)
        {
            this.cache = cache;
            this.publisher = publisher;
        }
        public override async Task<Result> Handle(SendConfirmationEmailCommand request, 
            CancellationToken cancellationToken)
        {
            var confirmationCode = RandomNumberGenerator.GetInt32(100000, 999999).ToString();

            var confirmMessage = new AuthConfiramtionEvent()
            {
                Recipient = request.Recipient,
                CreatedAt = DateTimeOffset.Now.ToString(),
                NotificationId = Guid.NewGuid(),
                MessageContent = new MessageContent() { Subject = "Код подтверждения", Text = confirmationCode }
            };

            var message = JsonSerializer.Serialize(confirmMessage);

            await cache.SetStringAsync(request.Recipient, confirmationCode);

            await publisher.SendMessageAsync(message, "email", cancellationToken);

            return Result.Success();
        }
    }
}
