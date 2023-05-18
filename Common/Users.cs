using Google.Cloud.Firestore;
using System.ComponentModel.DataAnnotations;

namespace Common
{

    [FirestoreData]
    public class Users : UserCredentials
    {
        [FirestoreProperty]
        [Required]
        public string Name { get; set; } = string.Empty;
        [FirestoreProperty]
        [Required]
        public string Surname { get; set; } = string.Empty;
    }
}