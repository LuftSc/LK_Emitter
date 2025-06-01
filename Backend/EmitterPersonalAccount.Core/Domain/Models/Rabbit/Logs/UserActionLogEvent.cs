using EmitterPersonalAccount.Core.Domain.SharedKernal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.Models.Rabbit.Logs
{
    public record UserActionLogEvent(
        Guid UserId,
        string Type,
        DateTime TimeStamp,
        string IpAddress = "",
        string AdditionalDataJSON = ""
        )
    {
    }
}
