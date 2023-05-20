using Common;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace eCommerceClient.DataAccess
{
    public class UsersService
    {
        private readonly HttpClient _httpClient;

        public UsersService()
        {
            _httpClient = new HttpClient();
        }


        public async Task<Users> SignUp(Users user)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"https://localhost:7254/User/signup", user);
            if (response.IsSuccessStatusCode)
            { 
                string jsonData = await response.Content.ReadAsStringAsync();
                try
                {
                    Users userSaved = JsonConvert.DeserializeObject<Users>(jsonData);
                    return userSaved;
                }
                catch (Exception ex)
                {
                    return null;
                }

            }
            throw new Exception("Failed to register user using the Customers Micro Service.");
        }

        public async Task<Users> SignIn(UserCredentials userCredentials)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("https://localhost:7254/User/signin", userCredentials);
            if (response.IsSuccessStatusCode)
            {
                string jsonData = await response.Content.ReadAsStringAsync();
                try
                {
                    Users userLogged = JsonConvert.DeserializeObject<Users>(jsonData);
                    return userLogged;
                }
                catch (Exception ex)
                {
                    return null;
                }

            }
            throw new Exception("Failed to sign in user using the Customers Micro Service.");
        }

        public async Task<Users> GetUser(string email)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"https://localhost:7254/User/{email}");
            if (response.IsSuccessStatusCode)
            {
                string jsonData = await response.Content.ReadAsStringAsync();
                try
                {
                    Users userLogged = JsonConvert.DeserializeObject<Users>(jsonData);
                    return userLogged;
                }
                catch (Exception ex)
                {
                    return null;
                }

            }
            throw new Exception("Failed to sign in user using the Customers Micro Service.");
        }

        public async Task<List<Notifications>> GetAllNotifications(string email)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"https://localhost:7254/User/notifications/{email}");
            if (response.IsSuccessStatusCode)
            {
                string jsonData = await response.Content.ReadAsStringAsync();
                try
                {
                    List<Notifications> notifications = JsonConvert.DeserializeObject<List<Notifications>>(jsonData);
                    return notifications;
                }
                catch (Exception ex)
                {
                    return null;
                }

            }
            throw new Exception("Failed to Retrieve All Notifications using the Customers Micro Service.");
        }
    }
}
