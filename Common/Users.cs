using Google.Cloud.Firestore;
using System.ComponentModel.DataAnnotations;

namespace Common
{

    [FirestoreData]
    public class Users : UserCredentials
    {
        [FirestoreProperty]
        [Required]
        public string userId { get; set; } = string.Empty;
        [FirestoreProperty]
        [Required]
        public string Name { get; set; } = string.Empty;
        [FirestoreProperty]
        [Required]
        public string Surname { get; set; } = string.Empty;
        [FirestoreProperty]
        [Required]
        public string prefCurrency { get; set; } = string.Empty;
        [FirestoreProperty]
        [Required]
        public string prefAddress { get; set; } = string.Empty;
        [FirestoreProperty]
        [Required]
        public int Credit { get; set; }
        [FirestoreProperty]
        [Required]
        public bool isAdmin { get; set; }
    }
}