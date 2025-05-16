using CSharpFunctionalExtensions;
using EmitterPersonalAccount.Core.Domain.Enums;
using EmitterPersonalAccount.Core.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Application.Services
{
    public class UsersService
    {
        private readonly IUserRepository userRepository;
        private readonly IEmittersRepository emittersRepository;

        public UsersService(IUserRepository userRepository, IEmittersRepository emittersRepository)
        {
            this.userRepository = userRepository;
            this.emittersRepository = emittersRepository;
        }

        /*public async Task<Result> AddUserRole(
            Guid userId, 
            Role role, 
            List<Guid>? emittersIdList, 
            CancellationToken cancellation)
        {
            if (role == Role.Emitter && emittersIdList is not null)
            {
                await 
            }
            await userRepository
                .AddWithRole(userId,
                role, cancellationToken);
        }*/
    }
}
