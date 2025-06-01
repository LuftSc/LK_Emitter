using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.Models.Rabbit.Logs
{
    public record GetUsersLogsEvent(
        string SenderId,
        Guid? UserId,
        DateTime? StartDate,
        DateTime? EndDate
        )
    {
    }
}
