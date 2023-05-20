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
            FromMessage = "{" + FromMessage.Split("{{")[1];
             _logger.LogInformation($"Split 1");
            Dictionary<string, object> orderData = JsonConvert.DeserializeObject<Dictionary<string, object>>(FromMessage.Split("Payments")[0]);
            FromMessage = "Payments" + FromMessage.Split("Payments")[1];
            // Deserialize the Payments object
             _logger.LogInformation($"Split 2");
            Dictionary<string, object> paymentData = JsonConvert.DeserializeObject<Dictionary<string, object>>(FromMessage.Split("Shippings")[0]);
            FromMessage = "Shippings" + FromMessage.Split("Shippings")[1];
            // Deserialize the Shippings object
             _logger.LogInformation($"Split 3");
            Dictionary<string, object> shippingData = JsonConvert.DeserializeObject<Dictionary<string, object>>(FromMessage.Split("Notifications")[0]);
            FromMessage = "Notifications" + FromMessage.Split("Notifications")[1];
            // Deserialize the Notifications object
            Dictionary<string, object> notificationData = JsonConvert.DeserializeObject<Dictionary<string, object>>(FromMessage.Split("}}")[0]);   
             _logger.LogInformation($"Split 4");
            
            _logger.LogInformation($"OrderId is {orderData["orderId"].ToString()}");

            
            var responseOrder =  _httpClient.PostAsJsonAsync($"https://localhost:7171/Order/AddOrder", orderData);
            responseOrder.Wait();
            var responsePayment =  _httpClient.PostAsJsonAsync($"https://localhost:7105/Payment/AddPayment", paymentData);
            responsePayment.Wait();
            var responseShipping =  _httpClient.PostAsJsonAsync($"https://localhost:7266/Shipping/AddShipping", shippingData);
            responseShipping.Wait();
            var responseNotification =  _httpClient.PostAsJsonAsync($"https://localhost:7254/User/AddNot", notificationData);
            responseNotification.Wait();
            }catch(Exception ex){
                _logger.LogInformation($"Exception: {ex}");
            }
            
            return Task.CompletedTask;
        }
    }

