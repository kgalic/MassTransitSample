using MassTransit;
using Microsoft.Azure.EventGrid.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MassTransitSample
{
    public class EventGridMessageConsumer : IConsumer<EventGridEvent>
    {
        public EventGridMessageConsumer()
        {
        }

        public Task Consume(ConsumeContext<EventGridEvent> context)
        {
            var jsonString = JsonConvert.SerializeObject(context.Message);
            Console.WriteLine($"Received {jsonString}");
            return Task.FromResult(true);
        }
    }
}
