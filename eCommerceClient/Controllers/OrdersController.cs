using Microsoft.AspNetCore.Mvc;

namespace eCommerceClient.Controllers
{
    public class OrdersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //Call pubsub function
        public async Task<IActionResult> AddOrder() { }
    }
}
