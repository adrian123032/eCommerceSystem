using eCommerceClient.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceClient.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        
        ProductsService _productsService;
        public ProductsController(ProductsService productsService)
        {
            _productsService = productsService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> List(int page, string search) { 
            var list = await _productsService.FetchProductData(page, search);
            return View(list);
        }
    }
}
