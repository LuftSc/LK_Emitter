using EmitterPersonalAccount.Application.Infrastructure.Cqs;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Repositories;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Application.Features.Authentification
{
    public sealed class RestorePasswordCommand : Command
    {
        public Guid UserId { get; set; }
        public string NewPassword { get; set; }
    }

    public sealed class RestorePasswordCommandHandler : CommandHandler<RestorePasswordCommand>
    {
        private readonly IPasswordHasher passwordHasher;
        private readonly IUserRepository userRepository;

        public RestorePasswordCommandHandler(IPasswordHasher passwordHasher, 
            IUserRepository userRepository)
        {
            this.passwordHasher = passwordHasher;
            this.userRepository = userRepository;
        }
        public override async Task<Result> Handle
            (RestorePasswordCommand request,
            CancellationToken cancellationToken)
        {
            var generatedHash = passwordHasher.Generate(request.NewPassword);

            return await userRepository
                .UpdatePassword(request.UserId, generatedHash);
        }
    }
}
