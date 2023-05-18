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
    public class Payments
    {
        [FirestoreProperty]
        [Required]
        public string paymentId { get; set; } = string.Empty;
        [FirestoreProperty]
        [Required]
        public string userEmail { get; set; } = string.Empty;
        [FirestoreProperty]
        [Required]
        public string orderId { get; set; } = string.Empty;
        [FirestoreProperty]
        [Required]
        public Timestamp paymentDT { get; set; }
        public DateTime dateTime
        {
            get { return paymentDT.ToDateTime(); }
            set { paymentDT = Timestamp.FromDateTime(value.ToUniversalTime()); }
        }
        [FirestoreProperty]
        [Required]
        public string paymentValue { get; set; } = string.Empty;
        [FirestoreProperty]
        [Required]
        public string currency { get; set; } = string.Empty;

    }
}
