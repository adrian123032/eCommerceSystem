using Common;
using eCommerceClient.DataAccess;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace eCommerceClient.Controllers
{
    public class OrdersController : Controller
    {
        OrdersService _ordersService;
        PubSubOrderRepository _pubsubRepo;
        public OrdersController(OrdersService ordersService, PubSubOrderRepository pubsubRepo)
        {
            _ordersService = ordersService;
            _pubsubRepo = pubsubRepo;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddOrder(string productId)
        {
            string userData = User.FindFirst(ClaimTypes.UserData)?.Value;
            string prefAddress = userData.Split("/")[1];
            return View(productId, prefAddress);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder(Orders order)
        {
            Shippings shipping = new Shippings();
            shipping.address = order.orderId;
            order.orderId = Guid.NewGuid().ToString();
            order.userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            order.statusCode = 1;
            return RedirectToAction("Index", "Home");
        }
    }
}
