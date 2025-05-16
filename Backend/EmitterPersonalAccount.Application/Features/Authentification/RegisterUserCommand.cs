using EmitterPersonalAccount.Application.Infrastructure.Cqs;
using EmitterPersonalAccount.Application.Services;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Enums;
using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.Models.Postgres.PartialModels;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit;
using EmitterPersonalAccount.Core.Domain.Repositories;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Application.Features.Authentification
{
    public sealed class RegisterUserCommand : Command
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public List<Guid>? EmittersGuids { get; set; }
        public Role Role { get; set; } = Role.User;
    }

    public sealed class RegisterUserCommandHandler : CommandHandler<RegisterUserCommand>
    {
        private readonly IPasswordHasher passwordHasher;
        private readonly IUserRepository userRepository;
        private readonly IRabbitMqPublisher publisher;
        private readonly IOutboxService outboxService;

        public RegisterUserCommandHandler(IPasswordHasher passwordHasher, 
            IUserRepository userRepository, 
            IRabbitMqPublisher publisher, 
            IOutboxService outboxService)
        {
            this.passwordHasher = passwordHasher;
            this.userRepository = userRepository;
            this.publisher = publisher;
            this.outboxService = outboxService;
        }
        public override async Task<Result> Handle
            (RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var passwordHash = passwordHasher.Generate(request.Password);

            var userCreateResult = User.Create(request.Email, passwordHash);

            if (!userCreateResult.IsSuccessfull) 
                return Error(new UserCreatingError());

            await userRepository
                .AddWithRole(userCreateResult.Value, 
                request.Role, request.EmittersGuids, cancellationToken);

            var outboxSavingResult = await outboxService
                .CreateAndSaveOutboxMessage(
                    OutboxMessageType.AddUser,
                    JsonSerializer.Serialize(Tuple.Create(
                        userCreateResult.Value.Id, 
                        request.EmittersGuids,
                        request.Role.ToString())),
                    cancellationToken);

            if (!outboxSavingResult.IsSuccessfull) return outboxSavingResult;

            return Result.Success();
        }
    }
    public class UserCreatingError : Error
    {
        public override string Type => nameof(UserCreatingError);
    }

    
}
