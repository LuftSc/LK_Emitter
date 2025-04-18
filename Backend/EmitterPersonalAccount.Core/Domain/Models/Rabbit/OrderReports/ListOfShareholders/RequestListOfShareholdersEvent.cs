using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmitterPersonalAccount.Core.Domain.SharedKernal.DTO;

namespace EmitterPersonalAccount.Core.Domain.Models.Rabbit.OrderReports.ListOfShareholders
{
    public record RequestListOfShareholdersEvent(
        DateTime SendingDate,
        ListOfShareholdersReportData RequestData,
        string UserId
        )
    { }
}
