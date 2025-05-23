using EmitterPersonalAccount.Application.Infrastructure.Cqs;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Enums;
using EmitterPersonalAccount.Core.Domain.Repositories;
using EmitterPersonalAccount.Core.Domain.SharedKernal.DTO;
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
        private readonly IProtectDataService protectService;

        public LoginUserQueryHandler(IUserRepository userRepository, 
            IMediator mediator, IPasswordHasher passwordHasher, 
            IProtectDataService protectService)
        {
            this.userRepository = userRepository;
            this.mediator = mediator;
            this.passwordHasher = passwordHasher;
            this.protectService = protectService;
        }
        public override async Task<Result<string>> Handle
            (LoginUserQuery request, CancellationToken cancellationToken)
        {
            var emailHash = protectService.HashForSearch(request.Email);


            var userGettingResult = await userRepository
                .GetUserWithRoles(u => u.EmailSearchHash == emailHash);

            if (!userGettingResult.IsSuccessfull) return
                    Error(new UserNotFoundError() 
                    { Data = { { "User", "User not found!!" } } });

            var isPasswordCorrect = passwordHasher
                .Verify(request.Password, userGettingResult.Value.PasswordHash);

            if (!isPasswordCorrect) 
                return Result<string>
                    .Error(new WrongUserPasswordError() { });

            var claimsData = new UserClaimsData(
                userGettingResult.Value.Id, 
                (Role)userGettingResult.Value.Roles.Max(r => r.Id));

            return Result<string>.Success(JsonSerializer.Serialize(claimsData));
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
