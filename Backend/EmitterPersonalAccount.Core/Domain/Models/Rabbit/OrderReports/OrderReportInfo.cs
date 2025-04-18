﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.Models.Rabbit.OrderReports
{
    public record OrderReportInfo(
        Guid Id,
        string FileName,
        string Status,
        DateTime RequestTime,
        Guid IdForDownload
        )
    {
    }
}
