using Common;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace eCommerceClient.DataAccess
{
    public class OrdersService
    {
        private readonly HttpClient _httpClient;

        public OrdersService()
        {
            _httpClient = new HttpClient();
        }

        //This function should be adapted to publish
        public async Task<Orders> AddOrder(Orders order)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"https://localhost:7171/Order/AddOrder", order);
            if (response.IsSuccessStatusCode)
            {
                string jsonData = await response.Content.ReadAsStringAsync();
                try
                {
                    Orders orderAdded = JsonConvert.DeserializeObject<Orders>(jsonData);
                    return orderAdded;
                }
                catch (Exception ex)
                {
                    return null;
                }

            }
            throw new Exception("Failed to Add Order using the Orders Micro Service.");
        }

        public async Task<List<Orders>> LoadOrders(string email)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"https://localhost:7171/Order/orders/{email}");
            if (response.IsSuccessStatusCode)
            {
                string jsonData = await response.Content.ReadAsStringAsync();
                try
                {
                    List<Orders> ordersList = JsonConvert.DeserializeObject<List<Orders>>(jsonData);
                    return ordersList;
                }
                catch (Exception ex)
                {
                    return null;
                }

            }
            throw new Exception("Failed to Retrieve Orders using the Orders Micro Service.");
        }

        public async Task<List<Orders>> GetAllOrders()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"https://localhost:7171/Order/allOrders");
            if (response.IsSuccessStatusCode)
            {
                string jsonData = await response.Content.ReadAsStringAsync();
                try
                {
                    List<Orders> orders = JsonConvert.DeserializeObject<List<Orders>>(jsonData);
                    return orders;
                }
                catch (Exception ex)
                {
                    return null;
                }

            }
            throw new Exception("Failed to Retrieve All Orders using the Orders Micro Service.");
        }

        public async Task<Orders> GetOrder(string orderId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"https://localhost:7171/Order/details/{orderId}");
            if (response.IsSuccessStatusCode)
            {
                string jsonData = await response.Content.ReadAsStringAsync();
                try
                {
                    Orders order = JsonConvert.DeserializeObject<Orders>(jsonData);
                    return order;
                }
                catch (Exception ex)
                {
                    return null;
                }

            }
            throw new Exception("Failed to Retrieve Order using the Orders Micro Service.");
        }
    }
}
