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
        public Guid UserId { get; private set; }
        public Guid EmitterId { get; private set; }
    }
}
