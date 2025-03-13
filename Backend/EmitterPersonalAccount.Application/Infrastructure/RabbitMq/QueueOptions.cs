using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Application.Infrastructure.RabbitMq
{
    public class QueueOptions
    {
        public string Name { get; set; } = string.Empty;
        public string RoutingKey { get; set; } = string.Empty;
    }
}
