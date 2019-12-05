
using System;
using System.Threading.Tasks;
using MassTransit;
using Newtonsoft.Json;
using MassTransit.Serialization;
using MassTransit.Azure.ServiceBus.Core;
using System.Net.Mime;

namespace MassTransitSample
{
    class Program
    {
        static string ContentTypeJson = "application/json";
        static async Task Main(string[] args)
        {
            var bus = Bus.Factory.CreateUsingAzureServiceBus(cfg =>
            {
                var queueName = "Your SB Queue Name";
                var connectionString = "Connection String with RooTManage policy";
                var host = cfg.Host(connectionString, h =>
                {
                    h.OperationTimeout = TimeSpan.FromSeconds(60);
                });

                cfg.ReceiveEndpoint(queueName,
                    e =>
                    {
                        e.AddMessageDeserializer(contentType: new ContentType(ContentTypeJson), () =>
                        {
                            return new EventGridMessgeDeserializer(ContentTypeJson);
                        });
                        e.Consumer(() => new EventGridMessageConsumer());

                        // Uncomment if required deserializer for local messages - mass transit as publisher or direct messages from SB
                        //e.AddMessageDeserializer(contentType: JsonMessageSerializer.JsonContentType, () =>
                        //{
                        //    return new CustomMessageDeserializer(JsonMessageSerializer.JsonContentType.ToString());
                        //});
                        //e.Consumer(() => new MessageConsumer());
                    });

            });
            bus.Start();

            Console.WriteLine("Press any key to exit");
            // for testing purposes of local messages - mass transit as publisher
            // await bus.Publish<CustomMessage>(new {  Hello = "Hello, World." });
            await Task.Run(() => Console.ReadKey());

            await bus.StopAsync();

        }
    }
}
