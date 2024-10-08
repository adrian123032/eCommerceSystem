﻿using RestSharp;

namespace ProductMicroService.DataAccess
{
    public class ScraperService
    {
        private const string BaseUrl = "https://ebay-data-scraper.p.rapidapi.com";
        private readonly HttpClient _httpClient;

        public ScraperService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Key", "9a216c588fmsh0853a1ae814bd4dp17c44ejsnc9494ab207b6");
        }

        public async Task<string> FetchProductData(int page, string search)
        {
            string url = $"{BaseUrl}/products?page_number={page}&product_name={search}&country=ireland";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
 
                return await response.Content.ReadAsStringAsync();
            }

            // Handle error cases if necessary
            throw new Exception("Failed to fetch product data from the Ebay Data Scraper API.");
        }

        public async Task<string> FetchProductDetails(string id)
        {
            string url = $"{BaseUrl}/products/{id}?country=ireland";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {

                return await response.Content.ReadAsStringAsync();
            }

            // Handle error cases if necessary
            throw new Exception("Failed to fetch product data from the Ebay Data Scraper API.");
        }
    }

}
