using EmitterPersonalAccount.Application.Infrastructure.Cqs;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Application.Features.Authentification
{
    public sealed class SendConfirmationCodeCommand : Command
    {
        public string RecipientEmail { get; set; }
    }
    public sealed class SendConfirmationCodeCommandHandler
        : CommandHandler<SendConfirmationCodeCommand>
    {
        private readonly IDistributedCache distributedCache;
        private readonly IRabbitMqPublisher publisher;

        public SendConfirmationCodeCommandHandler(IDistributedCache distributedCache, 
            IRabbitMqPublisher publisher)
        {
            this.distributedCache = distributedCache;
            this.publisher = publisher;
        }
        public override async Task<Result> Handle
            (SendConfirmationCodeCommand request, 
            CancellationToken cancellationToken)
        {
            var confirmationCode = RandomNumberGenerator.GetInt32(100000, 999999).ToString();

            var confirmMessage = new AuthConfiramtionEvent()
            {
                Recipient = request.RecipientEmail,
                CreatedAt = DateTimeOffset.Now.ToString(),
                NotificationId = Guid.NewGuid(),
                MessageContent = new MessageContent() { Subject = "Код подтверждения", Text = confirmationCode }
            };

            var message = JsonSerializer.Serialize(confirmMessage);

            await distributedCache
                .SetStringAsync(request.RecipientEmail, confirmationCode);

            await publisher
                .SendMessageAsync(message, 
                RabbitMqAction.SendEmailConfirmation.QueueName, 
                cancellationToken);

            return Result.Success();
        }
    }
}
