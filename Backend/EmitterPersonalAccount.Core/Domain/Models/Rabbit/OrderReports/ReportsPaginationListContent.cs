﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.Models.Rabbit.OrderReports
{
    public record ReportsPaginationListContent(
        OrderReportPaginationList OrderReports,
        string UserId
        )
    {

    }
}
