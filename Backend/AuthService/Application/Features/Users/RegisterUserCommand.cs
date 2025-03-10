using AuthService.Application.Features.Password;
using EmitterPersonalAccount.Application.Infrastructure.Cqs;
using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.Repositories;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using MediatR;

namespace AuthService.Application.Features.Users
{
    public sealed class RegisterUserCommand : Command
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public sealed class RegisterPasswordCommandHandler : CommandHandler<RegisterUserCommand>
    {
        private readonly IMediator mediator;
        private readonly IUserRepository userRepository;

        public RegisterPasswordCommandHandler(IMediator mediator, IUserRepository userRepository)
        {
            this.mediator = mediator;
            this.userRepository = userRepository;
        }
        public override async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var passwordHash = await mediator
                .Send(
                    new GeneratePasswordHashQuery { Password = request.Password }, 
                    cancellationToken);

            var userCreateResult = User.Create(request.Email, passwordHash.Value);

            if (!userCreateResult.IsSuccessfull) return Error(new UserCreatingError());

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
