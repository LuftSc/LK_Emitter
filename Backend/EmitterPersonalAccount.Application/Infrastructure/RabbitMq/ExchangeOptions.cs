using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Application.Infrastructure.RabbitMq
{
    public class ExchangeOptions
    {
        public string Name { get; set; } = string.Empty;
        public List<QueueOptions> Queues { get; set; } = [];
    }
}
