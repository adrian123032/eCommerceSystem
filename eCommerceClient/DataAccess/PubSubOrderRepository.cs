using Google.Cloud.PubSub.V1;
using Google.Protobuf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace eCommerceClient.DataAccess
{
    public class PubSubOrderRepository
    {

        TopicName topicName;
        Topic topic;
        public PubSubOrderRepository()
        {
            topicName = TopicName.FromProjectTopic("striking-audio-387012", "orders");
            if (topicName == null)
            {
                PublisherServiceApiClient publisher = PublisherServiceApiClient.Create();
                try
                {
                    topicName = new TopicName("striking-audio-387012", "orders");
                    topic = publisher.CreateTopic(topicName);
                }
                catch (Exception ex)
                {
                    //log
                    throw ex;
                }
            }
        }

        public async Task<string> PushMessage(Orders order, Payments payment, Shippings shipping, Notifications notification)
        {
            var combinedData = new Dictionary<string, object>
            {
                { "Orders", order },
                { "Payments", payment },
                { "Shippings", shipping },
                { "Notifications", notification }
            };
            PublisherClient publisher = await PublisherClient.CreateAsync(topicName);

            var pubsubMessage = new PubsubMessage
            {
                // The data is any arbitrary ByteString. Here, we're using text.
                Data = ByteString.CopyFromUtf8(JsonConvert.SerializeObject(combinedData)),
                // The attributes provide metadata in a string-to-string dictionary.
                Attributes =
                {
                    { "priority", "normal" }
                }
            };
            string message = await publisher.PublishAsync(pubsubMessage);
            return message;
        }

    }
}
