using EmitterPersonalAccount.Application.Infrastructure.Cqs;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.Repositories;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using EmitterPersonalAccount.DataAccess.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Application.Features.Authentification
{
    public sealed class RegisterUserCommand : Command
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public sealed class RegisterUserCommandHandler : CommandHandler<RegisterUserCommand>
    {
        private readonly IPasswordHasher passwordHasher;
        private readonly IUserRepository userRepository;

        public RegisterUserCommandHandler(IPasswordHasher passwordHasher, 
            IUserRepository userRepository)
        {
            this.passwordHasher = passwordHasher;
            this.userRepository = userRepository;
        }
        public override async Task<Result> Handle
            (RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var passwordHash = passwordHasher.Generate(request.Password);

            var userCreateResult = User.Create(request.Email, passwordHash);

            if (!userCreateResult.IsSuccessfull) 
                return Error(new UserCreatingError());

            await userRepository.AddAsync(userCreateResult.Value, cancellationToken);
            await userRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
    public class UserCreatingError : Error
    {
        public override string Type => nameof(UserCreatingError);
    }
}
