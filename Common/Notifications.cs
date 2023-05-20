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
    public class Notifications
    {
        [FirestoreProperty]
        [Required]
        public string email { get; set; } = string.Empty;
        [FirestoreProperty]
        [Required]
        public string description { get; set; } = string.Empty;
        [FirestoreProperty]
        [Required]
        public Timestamp notificationtDT { get; set; }
        public DateTime dateTime
        {
            get { return notificationtDT.ToDateTime(); }
            set { notificationtDT = Timestamp.FromDateTime(value.ToUniversalTime()); }
        }
    }
}
