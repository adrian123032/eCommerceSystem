using Common;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static Google.Cloud.Firestore.V1.StructuredQuery.Types;

namespace CustomerMicroService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly FirestoreDb _db;
        private readonly CollectionReference _usersCollection;

        public UserController(FirestoreDb db)
        {
            _db = db;
            _usersCollection = _db.Collection("users");
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(Users user)
        {

            DocumentReference document = await _usersCollection.AddAsync(user);
            user.userId = document.Id;
            DocumentReference uploadsRef = _usersCollection.Document(user.userId);
            await uploadsRef.SetAsync(user);
            return Ok(user);
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(UserCredentials credential)
        {
            // Authenticate the user with Firestore
            Query query = _usersCollection.WhereEqualTo("Email", credential.Email).WhereEqualTo("Password", credential.Password);
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
            if (querySnapshot.Count == 0)
            {
                return Unauthorized();
            }

            DocumentSnapshot documentSnapshot = querySnapshot.Documents[0];
            Users user = documentSnapshot.ConvertTo<Users>();
            return Ok(user);
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> GetUser(string email)
        {
            Query query = _usersCollection.WhereEqualTo("Email", email);
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
            if (querySnapshot.Count == 0)
            {
                return Unauthorized();
            }

            DocumentSnapshot documentSnapshot = querySnapshot.Documents[0];
            Users user = documentSnapshot.ConvertTo<Users>();
            return Ok(user);
        }
    }
}
