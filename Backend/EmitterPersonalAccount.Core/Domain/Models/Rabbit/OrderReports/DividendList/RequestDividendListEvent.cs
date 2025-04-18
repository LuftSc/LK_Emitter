using EmitterPersonalAccount.Core.Domain.SharedKernal.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.Models.Rabbit.OrderReports.DividendList
{
    public record RequestDividendListEvent(
        DateTime SendingDate,
        DividendListReportData RequestData,
        string UserId
        )
    {
    }
}
