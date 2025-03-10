using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.Models.Rabbit
{
    public class AuthConfiramtionEvent
    {
        public required Guid NotificationId { get; set; }
        public required string Recipient { get; set; }
        public required MessageContent MessageContent { get; set; }
        public required string CreatedAt { get; set; }

    }
}
