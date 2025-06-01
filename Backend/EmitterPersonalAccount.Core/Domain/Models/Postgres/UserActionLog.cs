//using CSharpFunctionalExtensions;
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
    public class UserActionLog : Entity<Guid>, IAggregateRoot
    {
        private UserActionLog() : base(Guid.NewGuid())
        {
        }
        private UserActionLog(
            Guid userId, 
            string action,
            DateTime timeStamp,
            string ipAddress,
            string additionalDataJSON) : base(Guid.NewGuid())
        {
            UserId = userId;
            ActionType = action;
            Timestamp = timeStamp;
            IpAddress = ipAddress;
            AdditionalDataJSON = additionalDataJSON;
        }
        public Guid UserId { get; private set; }
        public string ActionType { get; private set; }
        public DateTime Timestamp { get; private set; }
        public string IpAddress { get; private set; }
        public string AdditionalDataJSON { get; private set; } = string.Empty; // JSON

        public static Result<UserActionLog> Create(
            Guid userId,
            string action,
            DateTime timeStamp,
            string ipAddress = "",
            string additionalDataJSON = "")
        {
            return Result<UserActionLog>
                .Success(new UserActionLog(userId, action, timeStamp, ipAddress, additionalDataJSON));
        }
    }
}
