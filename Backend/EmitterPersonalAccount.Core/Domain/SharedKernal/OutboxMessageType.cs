using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.SharedKernal
{
    public enum OutboxMessageType
    {
        AddUser = 1,
        UpdateUser = 2,
        AddEmitter,
        UpdateEmitter,
        AddUserEmitterBinding
    }
}
