using Common;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;

namespace ShippingMicroService.Controllers
{
    public class ShippingController : Controller
    {
        private readonly FirestoreDb _db;
        private readonly CollectionReference _shippingsCollection;

        public ShippingController(FirestoreDb db)
        {
            _db = db;
            _shippingsCollection = _db.Collection("shippings");
        }

        [HttpPost("AddShipping")]
        public async Task<IActionResult> AddShipping(Shippings shipping)
        {
            // Save the user to Firestore
            DocumentReference document = await _shippingsCollection.AddAsync(shipping);
            shipping.shippingId = document.Id;
            DocumentReference shippingRef = _shippingsCollection.Document(shipping.shippingId);
            await shippingRef.SetAsync(shipping);
            return Ok(document.Id);
        }

        [HttpGet("shipping/{orderId}")]
        public async Task<IActionResult> LoadOrders(string orderId)
        {
            List<Shippings> payments = new List<Shippings>();
            Query allShippingsQuery = _shippingsCollection.WhereEqualTo("orderId", orderId);
            QuerySnapshot allShippingsQuerySnapshot = await allShippingsQuery.GetSnapshotAsync();
            foreach (DocumentSnapshot documentSnapshot in allShippingsQuerySnapshot.Documents)
            {
                Payments payment = documentSnapshot.ConvertTo<Payments>();
                payments.Add(payment);
            }
            return Ok(payments);
        }

        [HttpGet("paymentdetails/{paymentId}")]
        public async Task<IActionResult> GetOrder(string paymentId)
        {
            Query allPaymentsQuery = _paymentsCollection.WhereEqualTo("paymentId", paymentId);
            QuerySnapshot allPaymentsQuerySnapshot = await allPaymentsQuery.GetSnapshotAsync();
            try
            {
                DocumentSnapshot documentSnapshot = allPaymentsQuerySnapshot.Documents.FirstOrDefault();
                Payments paymentRet = documentSnapshot.ConvertTo<Payments>();
                return Ok(paymentRet);

            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
    }
}
