using Common;
using Newtonsoft.Json;
namespace eCommerceClient.DataAccess
{
    public class ProductsService
    {
        private readonly HttpClient _httpClient;

        public ProductsService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<ProductsSearch>> FetchProductData(int page, string search)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"https://localhost:7295/api/ebay/products/{search}?page={page}");
            if (response.IsSuccessStatusCode)
            {

                string jsonData = await response.Content.ReadAsStringAsync();
                try
                {
                    List<ProductsSearch> productsSearchList = JsonConvert.DeserializeObject<List<ProductsSearch>>(jsonData);
                    return productsSearchList;
                }
                catch (Exception ex) {
                    return null;
                }

            }

            throw new Exception("Failed to fetch product data from the Product Micro Service.");
        }

        public async Task<ProductsDetail> FetchProductDetails(string id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"https://localhost:7295/api/ebay/detail/{id}");
            if (response.IsSuccessStatusCode)
            {
                string jsonData = await response.Content.ReadAsStringAsync();
                try
                {
                    ProductsDetail productsDetail = JsonConvert.DeserializeObject<ProductsDetail>(jsonData);
                    return productsDetail;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }

            throw new Exception("Failed to fetch product details from the Product Micro Service.");
        }
    }
}
