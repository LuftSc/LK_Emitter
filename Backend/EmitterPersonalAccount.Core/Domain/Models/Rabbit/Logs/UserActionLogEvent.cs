using EmitterPersonalAccount.Core.Domain.SharedKernal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.Models.Rabbit.Logs
{
    public record UserActionLogEvent(
        string UserId,
        string Type,
        string IpAddress,
        string AdditionalDataJSON = ""
        )
    {
    }
}
