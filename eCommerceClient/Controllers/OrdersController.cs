using Common;
using eCommerceClient.DataAccess;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace eCommerceClient.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        OrdersService _ordersService;
        PubSubOrderRepository _pubsubRepo;
        ProductsService _productsService;
        public OrdersController(OrdersService ordersService, PubSubOrderRepository pubsubRepo, ProductsService productsService)
        {
            _ordersService = ordersService;
            _pubsubRepo = pubsubRepo;
            _productsService = productsService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddOrder(string product_id)
        {
            string userData = User.FindFirst(ClaimTypes.UserData)?.Value;
            string prefAddress = userData.Split("/")[1];
            Orders order =new Orders();
            order.productId = product_id;
            order.orderId = prefAddress;
            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder(Orders order)
        {
            Shippings shipping = new Shippings();
            shipping.address = order.orderId;
            order.orderId = Guid.NewGuid().ToString();
            order.userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            order.statusCode = 1;
            order.dateTime = DateTime.Now;

            shipping.shippingId = Guid.NewGuid().ToString();
            shipping.orderId = order.orderId;
            shipping.dtCreate = DateTime.Now;
            shipping.dtUpdate = DateTime.Now;
            shipping.statusCode = 1;

            Payments payment = new Payments();
            payment.paymentId = Guid.NewGuid().ToString();
            payment.orderId = order.orderId;
            payment.dateTime = DateTime.Now;
            payment.userEmail = order.userEmail;

            ProductsDetail product = await _productsService.FetchProductDetails(order.productId);
            payment.paymentValue = product.price;

            Notifications notification = new Notifications();
            notification.email = order.userEmail;
            notification.description = $"Product: {product.product_name}, was purchased for {product.price}";
            notification.dateTime = DateTime.Now;

            await _pubsubRepo.PushMessage(order, payment, shipping, notification);
            return RedirectToAction("Index", "Products");
        }
    }
}
