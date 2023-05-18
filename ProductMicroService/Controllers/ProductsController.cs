using Microsoft.AspNetCore.Mvc;
using ProductMicroService.DataAccess;

namespace ProductMicroService.Controllers
{
    [ApiController]
    [Route("api/amazon")]
    public class ProductsController : Controller
    {
        private readonly ScraperService _amazonService;

        public ProductsController(ScraperService amazonService)
        {
            _amazonService = amazonService;
        }

        [HttpGet("products/{search}")]
        public async Task<IActionResult> GetProductData(string search)
        {
            string productData = await _amazonService.FetchProductData(search);

            return Ok(productData);
        }

        [HttpGet("detail/{id}")]
        public async Task<IActionResult> GetProductDetails(string id)
        {
            string productData = await _amazonService.FetchProductDetails(id);

            return Ok(productData);
        }
    }
}
