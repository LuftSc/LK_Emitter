using EmitterPersonalAccount.Core.Domain.SharedKernal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.Models.Rabbit.OrderReports
{
    public record OrderReportDTO (
        Guid IdForDownload,
        Guid InternalId,
        string FileName,
        CompletionStatus Status,
        DateTime RequestDate,
        string UserId,
        ReportType Type
        )
    {
    }
}
