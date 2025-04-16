//using CSharpFunctionalExtensions;
using EmitterPersonalAccount.Core.Domain.Models.Postgres.EmitterModel;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.Models.Postgres
{
    public class OrderReport : Entity<Guid>, IAggregateRoot
    {
        private OrderReport() : base(Guid.NewGuid())
        {
            
        }
        private OrderReport(string fileName, DateTime requestDate) 
            : base(Guid.NewGuid()) 
        {
            ExternalStorageId = Guid.Empty;
            FileName = fileName;
            Status = CompletionStatus.Processing;
            RequestDate = requestDate;
        }
        public Guid ExternalStorageId { get; private set; }
        public string FileName { get; private set; }
        public CompletionStatus Status { get; private set; }
        public DateTime RequestDate { get; private set; }
        //public Emitter Emitter { get; private set; } = null!;
        public int IssuerId { get; private set; }
        public static Result<OrderReport> Create(string fileName, DateTime requestDate)
        {
            return Result<OrderReport>
                .Success(new OrderReport(fileName, requestDate));
        }
    }
}
