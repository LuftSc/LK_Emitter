using AuthService.Application.Features.Password;
using EmitterPersonalAccount.Application.Infrastructure.Cqs;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Repositories;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace AuthService.Application.Features.Users
{
    public sealed class LoginUserQuery : Query<string>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class ValidationUserError : Error
    { // Это класс для создания нормальной человеческой ошибки
        public override string Type => nameof(ValidationUserError);
        // Можно добавить ещё какую-то логику, которая будет
        // устанавливать этот тип при попадании значения
        public ValidationUserError(LoginUserQuery request)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
                Data[nameof(request.Email)] = $"Empty {nameof(request.Email)}";
            if (string.IsNullOrWhiteSpace(request.Password))
                Data[nameof(request.Password)] = $"Empty password {nameof(request.Password)}" ;
        }
    }
    public class WrongUserPasswordError : Error
    {
        public override string Type => nameof(WrongUserPasswordError);
    }

    public class UserNotFoundError : Error
    {
        public override string Type => nameof(UserNotFoundError);
    }
    public sealed class LoginUserCommandHandler : QueryHandler<LoginUserQuery, string>
    {
        private readonly IUserRepository userRepository;
        private readonly IMediator mediator;
        private readonly IJwtProvider jwtProvider;

        public LoginUserCommandHandler(IUserRepository userRepository, 
            IMediator mediator, IJwtProvider jwtProvider)
        {
            this.userRepository = userRepository;
            this.mediator = mediator;
            this.jwtProvider = jwtProvider;
        }
        public override async Task<Result<string>> Handle(LoginUserQuery request,
            CancellationToken cancellationToken)
        {
            var error = new ValidationUserError(request);
            if (error.Data.Count > 0) return Error(error);

            var user = await userRepository
                .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

            if (user == null) return
                    Error(new UserNotFoundError() { Data = { { "User", "User not found!!" } } });

            var verifyPasswordCommand = new VerifyPasswordHashCommand
            {
                Password = request.Password,
                HashedPassword = user.PasswordHash
            };

            var verifyResult = await mediator.Send(verifyPasswordCommand);

            if (!verifyResult.IsSuccessfull)
            {
                return Result<string>.Error(new WrongUserPasswordError() { });
            }

            return Result<string>.Success(user.Id.ToString());
        }
    }
}
