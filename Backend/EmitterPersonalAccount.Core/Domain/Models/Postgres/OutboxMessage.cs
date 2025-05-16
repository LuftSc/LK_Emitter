//using CSharpFunctionalExtensions;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.Models.Postgres
{
    public class OutboxMessage : Entity<Guid>
    {
        private OutboxMessage() : base(Guid.NewGuid())
        {
        }

        private OutboxMessage(
            OutboxMessageType type, 
            string contentJSON,
            bool isMessageHasBeenProcessed = false,
            string error = "")
            : base(Guid.NewGuid())
        {
            Type = type;
            ContentJSON = contentJSON;
            Timestamp = DateTime.Now.ToUniversalTime().AddHours(5);
            IsMessageHasBeenProcessed = isMessageHasBeenProcessed;
            Error = error;
        }

        public OutboxMessageType Type { get; private set; }
        public string ContentJSON { get; private set; }
        public DateTime Timestamp { get; private set; }
        public bool IsMessageHasBeenProcessed { get; private set; }    
        public string Error {  get; private set; }

        public static Result<OutboxMessage> Create(
            OutboxMessageType type, 
            string contentJSON, 
            bool isMessageHasBeenProcessed = false,
            string error = "")
        {
            return Result<OutboxMessage>
                .Success(new OutboxMessage(type, contentJSON, isMessageHasBeenProcessed, error));
        }
    }
}
