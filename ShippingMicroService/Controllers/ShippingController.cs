using Common;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;

namespace ShippingMicroService.Controllers
{
    [ApiController]
    [Route("[controller]")]
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


        [HttpGet("shippingdetails/{orderId}")]
        public async Task<IActionResult> GetOrder(string orderId)
        {
            Query allShippingsQuery = _shippingsCollection.WhereEqualTo("orderId", orderId);
            QuerySnapshot allShippingsQuerySnapshot = await allShippingsQuery.GetSnapshotAsync();
            try
            {
                DocumentSnapshot documentSnapshot = allShippingsQuerySnapshot.Documents.FirstOrDefault();
                Shippings shippingRet = documentSnapshot.ConvertTo<Shippings>();
                return Ok(shippingRet);

            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet("shippingupdate/{orderId}")]
        public async Task<IActionResult> UpdateShipping(string orderId)
        {
            Query allShippingsQuery = _shippingsCollection.WhereEqualTo("orderId", orderId);
            QuerySnapshot allShippingsQuerySnapshot = await allShippingsQuery.GetSnapshotAsync();
            try
            {
                DocumentSnapshot documentSnapshot = allShippingsQuerySnapshot.Documents.FirstOrDefault();
                Shippings shippingRet = documentSnapshot.ConvertTo<Shippings>();
                shippingRet.statusCode++;
                DocumentReference shippingsRef =_shippingsCollection.Document(documentSnapshot.Id);
                await shippingsRef.SetAsync(shippingRet);
                return Ok(shippingRet.statusCode);

            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
    }
}
