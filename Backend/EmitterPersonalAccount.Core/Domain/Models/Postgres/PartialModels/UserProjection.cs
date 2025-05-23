using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.Models.Postgres.PartialModels
{
    public class UserProjection : Entity<Guid>, IAggregateRoot
    {
        private UserProjection() : base(Guid.NewGuid())
        {
        }

        private UserProjection(Guid id, string role) : base(id)
        {
            Role = role;
        }
        public string Role { get; private set; } = string.Empty;
        public static Result<UserProjection> Create(Guid id, string role)
        {
            return Result<UserProjection>.Success(new UserProjection(id, role));
        }

        public List<EmitterProjection> Emitters { get; private set; } = [];
    }
}
