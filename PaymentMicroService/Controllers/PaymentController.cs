using Common;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;

namespace PaymentMicroService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : Controller
    {
        private readonly FirestoreDb _db;
        private readonly CollectionReference _paymentsCollection;

        public PaymentController(FirestoreDb db)
        {
            _db = db;
            _paymentsCollection = _db.Collection("payments");
        }

        [HttpPost("AddPayment")]
        public async Task<IActionResult> AddOrder(Payments payment)
        {
            // Save the user to Firestore
            DocumentReference document = await _paymentsCollection.AddAsync(payment);
            payment.paymentId = document.Id;
            DocumentReference paymentsRef = _paymentsCollection.Document(payment.paymentId);
            await paymentsRef.SetAsync(payment);
            return Ok(document.Id);
        }

        [HttpGet("payments/{email}")]
        public async Task<IActionResult> LoadOrders(string email)
        {
            List<Payments> payments = new List<Payments>();
            Query allPaymentsQuery = _paymentsCollection.WhereEqualTo("userEmail", email);
            QuerySnapshot allPaymentsQuerySnapshot = await allPaymentsQuery.GetSnapshotAsync();
            foreach (DocumentSnapshot documentSnapshot in allPaymentsQuerySnapshot.Documents)
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
