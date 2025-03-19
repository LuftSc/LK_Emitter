using AuthService.Application.Features.Password;
using EmitterPersonalAccount.Application.Infrastructure.Cqs;
using EmitterPersonalAccount.Core.Domain.Repositories;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using MediatR;

namespace AuthService.Application.Features.Users
{
    public sealed class RestorePasswordCommand : Command
    {
        public Guid UserId { get; set; }
        public string NewPassword { get; set; } = string.Empty;
    }

    public sealed class RestorePasswordCommandHandler : CommandHandler<RestorePasswordCommand>
    {
        private readonly IUserRepository userRepository;
        private readonly IMediator mediator;

        public RestorePasswordCommandHandler(IUserRepository userRepository,
            IMediator mediator)
        {
            this.userRepository = userRepository;
            this.mediator = mediator;
        }
        public override async Task<Result>
            Handle(RestorePasswordCommand request, CancellationToken cancellationToken)
        {
            var generateHashResult = await mediator
                .Send( new GeneratePasswordHashQuery() 
                    { Password = request.NewPassword });

            if (!generateHashResult.IsSuccessfull)
                return generateHashResult;

            return await userRepository
                .UpdatePassword(request.UserId, generateHashResult.Value);
        }
    }
}
