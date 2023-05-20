using Common;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        private readonly CollectionReference _notificationsCollection;


        public UserController(FirestoreDb db)
        {
            _db = db;
            _usersCollection = _db.Collection("users");
            _notificationsCollection = _db.Collection("notifications");
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
        public async Task<IActionResult> SignIn(UserCredentials credentialString)
        {
            Query query = _usersCollection.WhereEqualTo("Email", credentialString.Email).WhereEqualTo("Password", credentialString.Password);
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

        [HttpPost("AddNot")]
        public async Task<IActionResult> AddNot(Notifications notification)
        {
            DocumentReference document = await _notificationsCollection.AddAsync(notification);
            return Ok(notification);
        }

        [HttpGet("notifications/{email}")]
        public async Task<IActionResult> LoadNotifications(string email)
        {
            List<Notifications> notifications = new List<Notifications>();
            Query allNotificationsQuery = _notificationsCollection.WhereEqualTo("email", email);
            QuerySnapshot allNotificationsQuerySnapshot = await allNotificationsQuery.GetSnapshotAsync();
            foreach (DocumentSnapshot documentSnapshot in allNotificationsQuerySnapshot.Documents)
            {
                Notifications notification = documentSnapshot.ConvertTo<Notifications>();
                notifications.Add(notification);
            }

            return Ok(notifications);
        }

    }
}
