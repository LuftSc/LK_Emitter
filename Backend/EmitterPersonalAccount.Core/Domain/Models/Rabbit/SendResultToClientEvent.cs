using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.Models.Rabbit
{
    public class SendResultToClientEvent
    {
        public required string MethodForResultSending { get; set; }
        public required string ContentJSON { get; set; }
    }
}
