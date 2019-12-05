using MassTransit;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MassTransitSample
{
    public class MessageConsumer : IConsumer<CustomMessage>
    {
        public MessageConsumer()
        {
        }

        public Task Consume(ConsumeContext<CustomMessage> context)
        {
            var jsonString = JsonConvert.SerializeObject(context.Message);
            Console.WriteLine($"Received {jsonString}");
            return Task.FromResult(true);
        }
    }
}
