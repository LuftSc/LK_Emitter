using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.Models.Rabbit.OrderReports
{
    public class GetOrderReportsEvent
    {
        public required int IssuerId { get; set; }
        public required string UserId { get; set; }
        public required int Page { get; set; }
        public required int PageSize { get; set; }
    }
}
