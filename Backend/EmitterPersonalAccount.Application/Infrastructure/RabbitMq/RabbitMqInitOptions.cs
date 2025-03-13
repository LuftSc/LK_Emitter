using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Application.Infrastructure.RabbitMq
{
    public class RabbitMqInitOptions
    {
        public string RabbitMqUri { get; set; } = string.Empty;
        public List<ExchangeOptions> Exchanges { get; set; } = [];
    }
}
