using EmitterPersonalAccount.Application.Infrastructure.Cqs;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit.Logs;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Application.Features.Authentification
{
    public sealed class VerifyConfirmationCodeQuery : Query<string>
    {
        public string Email { get; set; }
        public string ConfiramtionCode { get; set; }
        public string IpAddress { get; set; } = string.Empty;
    }

    public sealed class VerifyConfirmationCodeQueryHandler
        : QueryHandler<VerifyConfirmationCodeQuery, string>
    {
        private readonly IDistributedCache distributedCache;
        private readonly IJwtProvider jwtProvider;
        private readonly IRabbitMqPublisher publisher;

        public VerifyConfirmationCodeQueryHandler(IDistributedCache distributedCache,
            IJwtProvider jwtProvider, IRabbitMqPublisher publisher)
        {
            this.distributedCache = distributedCache;
            this.jwtProvider = jwtProvider;
            this.publisher = publisher;
        }
        public override async Task<Result<string>> Handle
            (VerifyConfirmationCodeQuery request, 
            CancellationToken cancellationToken)
        {
            // Ещё можно как-то запускать таймер(например, 5 минут, и через это время удалять код)
            var savedCode = await distributedCache.GetStringAsync(request.Email);

            if (savedCode != request.ConfiramtionCode)
                return Error(new WrongConfirmationCodeError());

            var userId = await distributedCache.GetStringAsync($"id-{request.Email}");

            if (userId is null)
                 return Error(new ExpiredConfirmationCodeError());

            await distributedCache.RemoveAsync(request.Email);
            await distributedCache.RemoveAsync($"id-{request.Email}");

            var loggingActionEvent = new UserActionLogEvent
                (userId, ActionLogType.LoginToSystem.Type, request.IpAddress);

            var deliveryResult = await publisher.SendMessageAsync(
                JsonSerializer.Serialize(loggingActionEvent),
                RabbitMqAction.Audit,
                default);

            var token = jwtProvider.GenerateToken(userId);

            return Success(token);
        }
    }

    public class WrongConfirmationCodeError : Error
    {
        public override string Type => nameof(WrongConfirmationCodeError);
    }
    public class ExpiredConfirmationCodeError : Error
    {
        public override string Type => nameof(ExpiredConfirmationCodeError);
    }
}
