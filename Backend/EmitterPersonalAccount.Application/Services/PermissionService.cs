using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Enums;
using EmitterPersonalAccount.Core.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Application.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IUserRepository usersRepository;

        public PermissionService(IUserRepository usersRepository)
        {
            this.usersRepository = usersRepository;
        }
        public async Task<HashSet<Permission>> GetPermissionAsync(Guid userId)
        {
            return await usersRepository.GetUserPermissions(userId);
        }
    }
}
