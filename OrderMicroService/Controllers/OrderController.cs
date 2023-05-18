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
            // Save the user to Firestore
            DocumentReference document = await _ordersCollection.AddAsync(order);
            return Ok(document.Id);
        }

        [HttpGet("orders/{email}")]
        public async Task<IActionResult> LoadOrders(string email)
        {
            List<Orders> uploads = new List<Orders>();
            Query allOrdersQuery = _ordersCollection.WhereEqualTo("userEmail", email);
            QuerySnapshot allOrdersQuerySnapshot = await allOrdersQuery.GetSnapshotAsync();
            foreach (DocumentSnapshot documentSnapshot in allOrdersQuerySnapshot.Documents)
            {
                Orders order = documentSnapshot.ConvertTo<Orders>();
                    uploads.Add(order);
            }

            return Ok(uploads);
        }

        [HttpGet("details/{order}")]
        public async Task<IActionResult> GetOrder(Orders order)
        {
            Query allOrdersQuery = _ordersCollection.WhereEqualTo("userEmail", order.userEmail);
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
    }
}
