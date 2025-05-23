using EmitterPersonalAccount.Core.Domain.SharedKernal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.Models.Rabbit
{
    public sealed class SendResultToClientEvent
    {
        public required MethodResultSending MethodForResultSending { get; set; }
        public required string ContentJSON { get; set; }
    }
}
