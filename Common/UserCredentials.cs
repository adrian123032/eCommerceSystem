using Google.Cloud.Firestore;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [FirestoreData]
    public class UserCredentials
    {
        [FirestoreProperty]
        [Required]
        public string Email { get; set; } = string.Empty;
        [FirestoreProperty]
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
