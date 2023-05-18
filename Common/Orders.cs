using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{

    [FirestoreData]
    public class Orders
    {
        [FirestoreProperty]
        [Required]
        public string userEmail { get; set; } = string.Empty;
        [FirestoreProperty]
        [Required]
        public string productId { get; set; } = string.Empty;
        [FirestoreProperty]
        [Required]
        public Timestamp orderDT { get; set; }
        public DateTime dateTime
        {
            get { return orderDT.ToDateTime(); }
            set { orderDT = Timestamp.FromDateTime(value.ToUniversalTime()); }
        }
        [FirestoreProperty]
        [Required]
        public string paymentType { get; set; } = string.Empty;
        [FirestoreProperty]
        [Required]
        public int statusCode { get; set; }

    }
}
