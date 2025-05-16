using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.Models.Postgres
{
    // Связующая таблица
    public class UserEmitter
    {
        private UserEmitter()
        {
            
        }
        private UserEmitter(Guid userId, Guid emitterId)
        {
            UserId = userId;
            EmitterId = emitterId;
        }
        public Guid UserId { get; private set; }
        public Guid EmitterId { get; private set; }

        public static Result<UserEmitter> Create(Guid userId, Guid emitterId)
        {
            return Result<UserEmitter>.Success(new UserEmitter(userId, emitterId));
        }
    }
}
