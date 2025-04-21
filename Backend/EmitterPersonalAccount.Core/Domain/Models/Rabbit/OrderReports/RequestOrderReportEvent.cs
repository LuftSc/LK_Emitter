using EmitterPersonalAccount.Core.Domain.SharedKernal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.Models.Rabbit.OrderReports
{
    public class RequestOrderReportEvent
    {
        public required DateTime SendingDate { get; set; }
        public required ReportType ReportType { get; set; }
        public required string RequestDataJSON { get; set; }
        public required string UserId { get; set; } 
    }
}
