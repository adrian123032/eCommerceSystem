using Common;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;

namespace OrderMicroService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : Controller
    {
        private readonly FirestoreDb _db;
        private readonly CollectionReference _ordersCollection;

        public OrderController(FirestoreDb db)
        {
            _db = db;
            _ordersCollection = _db.Collection("orders");
        }

        [HttpPost("AddOrder")]
        public async Task<IActionResult> AddOrder(Orders order)
        {
            DocumentReference document = await _ordersCollection.AddAsync(order);
            order.orderId = document.Id;
            DocumentReference uploadsRef = _ordersCollection.Document(order.orderId);
            await uploadsRef.SetAsync(order);
            return Ok(document.Id);
        }

        [HttpGet("orders/{email}")]
        public async Task<IActionResult> LoadOrders(string email)
        {
            List<Orders> orders = new List<Orders>();
            Query allOrdersQuery = _ordersCollection.WhereEqualTo("userEmail", email);
            QuerySnapshot allOrdersQuerySnapshot = await allOrdersQuery.GetSnapshotAsync();
            foreach (DocumentSnapshot documentSnapshot in allOrdersQuerySnapshot.Documents)
            {
                Orders order = documentSnapshot.ConvertTo<Orders>();
                    orders.Add(order);
            }

            return Ok(orders);
        }

        [HttpGet("allOrders")]
        public async Task<IActionResult> GetAllOrders()
        {
            List<Orders> orders = new List<Orders>();
            Query allOrdersQuery = _ordersCollection;
            QuerySnapshot allOrdersQuerySnapshot = await allOrdersQuery.GetSnapshotAsync();
            foreach (DocumentSnapshot documentSnapshot in allOrdersQuerySnapshot.Documents)
            {
                Orders order = documentSnapshot.ConvertTo<Orders>();
                orders.Add(order);
            }

            return Ok(orders);
        }

        [HttpGet("details/{orderId}")]
        public async Task<IActionResult> GetOrder(string orderId)
        {
            Query allOrdersQuery = _ordersCollection.WhereEqualTo("orderId", orderId);
            QuerySnapshot allOrdersQuerySnapshot = await allOrdersQuery.GetSnapshotAsync();
            try
            {
                DocumentSnapshot documentSnapshot = allOrdersQuerySnapshot.Documents.FirstOrDefault();
                Orders orderRet = documentSnapshot.ConvertTo<Orders>();
                return Ok(orderRet);

            }catch (Exception ex)
            {
                return Ok(ex);
            }            
        }

        [HttpGet("orderupdate/{orderId}")]
        public async Task<IActionResult> UpdateOrders(string orderId)
        {
            Query allOrdersQuery = _ordersCollection.WhereEqualTo("orderId", orderId);
            QuerySnapshot allOrdersQuerySnapshot = await allOrdersQuery.GetSnapshotAsync();
            try
            {
                DocumentSnapshot documentSnapshot = allOrdersQuerySnapshot.Documents.FirstOrDefault();
                Orders orderRet = documentSnapshot.ConvertTo<Orders>();
                orderRet.statusCode++;
                DocumentReference ordersRef = _ordersCollection.Document(documentSnapshot.Id);
                await ordersRef.SetAsync(orderRet);
                return Ok(orderRet.statusCode);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
    }
}
