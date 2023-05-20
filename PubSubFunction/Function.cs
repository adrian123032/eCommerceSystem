using CloudNative.CloudEvents;
using Google.Cloud.Functions.Framework;
using Google.Events.Protobuf.Cloud.PubSub.V1;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Google.Cloud.Firestore;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Net.Http.Json;

namespace PubSubFunction;

    public class Function : ICloudEventFunction<MessagePublishedData>
    {
        private readonly ILogger<Function> _logger;
        private readonly HttpClient _httpClient;

        public Function(ILogger<Function> logger)
        {
             _logger = logger; 
             _httpClient = new HttpClient();
        }

        public Task HandleAsync(CloudEvent cloudEvent, MessagePublishedData data, CancellationToken cancellationToken)
        {
            _logger.LogInformation("PubSub function has started executing");
            var FromMessage = data.Message?.TextData;
            _logger.LogInformation($"Data received is {FromMessage}");
            try{
            //FromMessage = FromMessage.Split("Orders\": ")[1];
            var dataJson = JsonConvert.DeserializeObject<Dictionary<string, object>>(FromMessage);
             _logger.LogInformation($"Attempting retrieve Orders");
            var orders = JsonConvert.DeserializeObject<Dictionary<string, object>>(dataJson["Orders"].ToString());
             _logger.LogInformation($"Orders: {orders}");
             _logger.LogInformation($"Attempting retrieve Payments");
            var payments = JsonConvert.DeserializeObject<Dictionary<string, object>>(dataJson["Payments"].ToString());            
             _logger.LogInformation($"Payments: {payments}");
             _logger.LogInformation($"Attempting retrieve Shippings");
            var shippings = JsonConvert.DeserializeObject<Dictionary<string, object>>(dataJson["Shippings"].ToString());             
             _logger.LogInformation($"Shippings: {shippings}");
             _logger.LogInformation($"Attempting retrieve Notifications");
            var notifications = JsonConvert.DeserializeObject<Dictionary<string, object>>(dataJson["Notifications"].ToString());
            _logger.LogInformation($"Notifications: {notifications}");
             /*_logger.LogInformation($"Attempting to process: {FromMessage.Split(",\n\"Payments\": ")[0]}");
            Dictionary<string, object> orderData = JsonConvert.DeserializeObject<Dictionary<string, object>>(FromMessage.Split(",\n\"Payments\"")[0]);
            FromMessage = FromMessage.Split(",\n\"Payments\": ")[1];
            // Deserialize the Payments object
             _logger.LogInformation($"Split 2");
             _logger.LogInformation($"Attempting to process: {FromMessage.Split(",\n\"Shippings\": ")[0]}");
            Dictionary<string, object> paymentData = JsonConvert.DeserializeObject<Dictionary<string, object>>(FromMessage.Split(",\n\"Shippings\"")[0]);
            FromMessage = FromMessage.Split(",\n\"Shippings\": ")[1];
            // Deserialize the Shippings object
             _logger.LogInformation($"Split 3");
             _logger.LogInformation($"Attempting to process: {FromMessage.Split(",\n\"Notifications\": ")[0]}");
            Dictionary<string, object> shippingData = JsonConvert.DeserializeObject<Dictionary<string, object>>(FromMessage.Split(",\n\"Notifications\"")[0]);
            FromMessage = FromMessage.Split(",\n\"Notifications\": ")[1];
            _logger.LogInformation($"Split 4"); 
            _logger.LogInformation($"Attempting to process: {FromMessage.Split("}\n}")[0]}");
            // Deserialize the Notifications object
            Dictionary<string, object> notificationData = JsonConvert.DeserializeObject<Dictionary<string, object>>(FromMessage.Split("}\n}")[0]);   
            */
             
            
            _logger.LogInformation($"OrderId is {orders["orderId"].ToString()}");

            FirestoreDb db = FirestoreDb.Create("striking-audio-387012");
            CollectionReference orderDb = db.Collection("orders");
            CollectionReference paymentDb = db.Collection("payments");
            CollectionReference shippingDb = db.Collection("shippings");
            CollectionReference notificationDb = db.Collection("notifications");

            _logger.LogInformation($"Adding Order");
            //Add Order
            var documentO = orderDb.AddAsync(orders);
            documentO.Wait();
            _logger.LogInformation($"Adding Order Complete");

            _logger.LogInformation($"Adding Payment");
            //Add Payment
            var documentP = paymentDb.AddAsync(payments);
            documentP.Wait();
            _logger.LogInformation($"Adding Payment Complete");

            _logger.LogInformation($"Adding Shipping");
            //Add Shipping
            var documentS = shippingDb.AddAsync(shippings);
            documentS.Wait();
            _logger.LogInformation($"Adding Shipping Complete");

            _logger.LogInformation($"Adding Notification");
            //Add Notification
            var n = notificationDb.AddAsync(notifications);
            n.Wait();
            _logger.LogInformation($"Adding Notification Complete");


            }catch(Exception ex){
                _logger.LogInformation($"Exception: {ex}");
            }
            
            return Task.CompletedTask;
        }
    }

