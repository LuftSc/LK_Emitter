using AuthService.Application.Features.Users;
using EmitterPersonalAccount.Application.Infrastructure.Cqs;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;

namespace AuthService.Application.Features.Password
{
    public class VerifyPasswordHashCommand : Command
    {
        public string Password { get; set; } 
        public string HashedPassword { get; set; }
    }

    public class InvalidPasswordError : Error
    {
        public override string Type => nameof(InvalidPasswordError);
    }

    public sealed class VerifyPasswordHashCommandHandler
        : CommandHandler<VerifyPasswordHashCommand>
    {
        public override Task<Result> Handle(VerifyPasswordHashCommand request, 
            CancellationToken cancellationToken)
        {
            var isSuccess = BCrypt.Net.BCrypt
                .EnhancedVerify(request.Password, request.HashedPassword);

            if (!isSuccess)
                return Task.FromResult( Error(new InvalidPasswordError() 
                    { Data = { { nameof(request.Password), "User password invalid" } } }));

            return Task.FromResult(Success());
        }
    }
}
