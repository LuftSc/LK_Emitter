using EmitterPersonalAccount.Core.Domain.SharedKernal.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.Models.Rabbit.OrderReports.ReeRepNotSign
{
    public record RequestReeRepNotSignEvent (
        DateTime SendingDate,
        ReeRepNotSignReportData RequestData,
        string UserId
        )
    {
    }
}
