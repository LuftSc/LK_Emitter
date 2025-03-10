using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BaseMicroservice
{
    public static class EventDeserializer<TEvent>
    {
        public static TEvent Deserialize(BasicDeliverEventArgs args)
        {
            var body = args.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var ev = JsonSerializer.Deserialize<TEvent>(message);

            return ev;
        }
    }
}
