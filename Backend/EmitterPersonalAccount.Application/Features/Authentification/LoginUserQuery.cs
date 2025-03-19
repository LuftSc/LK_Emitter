using EmitterPersonalAccount.Application.Infrastructure.Cqs;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Repositories;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Application.Features.Authentification
{
    public sealed class LoginUserQuery : Query<string>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public sealed class LoginUserQueryHandler : QueryHandler<LoginUserQuery, string>
    {
        private readonly IUserRepository userRepository;
        private readonly IMediator mediator;
        private readonly IPasswordHasher passwordHasher;

        public LoginUserQueryHandler(IUserRepository userRepository, 
            IMediator mediator, IPasswordHasher passwordHasher)
        {
            this.userRepository = userRepository;
            this.mediator = mediator;
            this.passwordHasher = passwordHasher;
        }
        public override async Task<Result<string>> Handle
            (LoginUserQuery request, CancellationToken cancellationToken)
        {
            var user = await userRepository
                .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

            if (user == null) return
                    Error(new UserNotFoundError() 
                    { Data = { { "User", "User not found!!" } } });

            var isPasswordCorrect = passwordHasher
                .Verify(request.Password, user.PasswordHash);

            if (!isPasswordCorrect) 
                return Result<string>
                    .Error(new WrongUserPasswordError() { });

            return Result<string>.Success(user.Id.ToString());
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
}
