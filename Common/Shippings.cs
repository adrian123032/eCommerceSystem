using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Shippings
    {
        [FirestoreProperty]
        [Required]
        public string shippingId { get; set; } = string.Empty;
        [FirestoreProperty]
        [Required]
        public string orderId { get; set; } = string.Empty;
        [FirestoreProperty]
        [Required]
        public Timestamp updateDT { get; set; }
        public Timestamp createdDT { get; set; }
        public DateTime dtUpdate
        {
            get { return updateDT.ToDateTime(); }
            set { updateDT = Timestamp.FromDateTime(value.ToUniversalTime()); }
        }
        public DateTime dtCreate
        {
            get { return createdDT.ToDateTime(); }
            set { createdDT = Timestamp.FromDateTime(value.ToUniversalTime()); }
        }
        [FirestoreProperty]
        [Required]
        public int statusCode { get; set; }
    }
}
