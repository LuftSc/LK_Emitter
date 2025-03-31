using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.SharedKernal
{
    /*public class ReportOrderStatus : Entity<long>
    {
        public static readonly ReportOrderStatus Successfull = new(1, "Successfull");
        public static readonly ReportOrderStatus Processing = new(2, "Processing");
        public static readonly ReportOrderStatus Failed = new(3, "Failed");
        private ReportOrderStatus(long id, string statusName) : base(id)
        {
            StatusName = statusName;
        }
        public string StatusName { get; private set; }

    }*/
    public enum ReportOrderStatus
    {
        Successfull = 1,
        Processing,
        Failed
    }
}
