using GreenPipes;
using MassTransit;
using MassTransit.Azure.ServiceBus.Core.Contexts;
using MassTransit.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Text;
using System.Threading;

namespace MassTransitSample
{
    public class CustomMessageDeserializer : IMessageDeserializer
    {
        private string _contentType;

        public CustomMessageDeserializer(string contentType)
        {
            _contentType = contentType;
        }
        public ContentType ContentType => new ContentType(_contentType);

        public ConsumeContext Deserialize(ReceiveContext receiveContext)
        {
            var body = Encoding.UTF8.GetString(receiveContext.GetBody());
            var customMessage = JsonConvert.DeserializeObject<CustomMessage>(body);
            var serviceBusSendContext = new AzureServiceBusSendContext<CustomMessage>(customMessage, CancellationToken.None);
            
            string[] messageTypes = { "urn:message:MassTransitSample:CustomMessage" };
            var serviceBusContext = receiveContext as ServiceBusReceiveContext;
            serviceBusSendContext.ContentType = new ContentType(JsonMessageSerializer.JsonContentType.ToString());
            serviceBusSendContext.SourceAddress = serviceBusContext.InputAddress;
            serviceBusSendContext.SessionId = serviceBusContext.SessionId;

            // sending JToken because we are using default Newtonsoft deserializer/serializer
            var messageEnv = new JsonMessageEnvelope(serviceBusSendContext, JObject.Parse(body), messageTypes);
            return new JsonConsumeContext(JsonSerializer.CreateDefault(), receiveContext, messageEnv);
        }

        public void Probe(ProbeContext context)
        {
        }
    }
}

