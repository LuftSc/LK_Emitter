using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.Models.Postgres
{
    public class ActionsReport : Entity<Guid>, IAggregateRoot
    {
        private ActionsReport() : base(Guid.NewGuid())
        {
        }
        private ActionsReport(string title, byte[] content) : base(Guid.NewGuid())
        {
            Title = title;
            Content = content;
            DateOfGeneration = DateTime.Now.ToUniversalTime().AddHours(5);
        }
        public string Title { get; private set; } = string.Empty;
        public byte[] Content { get; private set; } = [];
        public DateTime DateOfGeneration { get; private set; }
        public double GetSize() => Math.Round(Content.Length / 1024.0, 0);

        public static Result<ActionsReport> Create(string title, byte[] content) 
        { 
            return Result<ActionsReport>.Success(new ActionsReport(title, content));
        }
    }
}
