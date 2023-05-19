using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class ProductsDetail
    {
        public string product_id { get; set; } = string.Empty;
        public string product_name { get; set; } = string.Empty;
        public string link { get; set; } = string.Empty;
        public string quantity_available { get; set; } = string.Empty;
        public string price { get; set; } = string.Empty; //Will require regex to get relevant part
        public string discounted_price { get; set; } = string.Empty;
        public string logistics_cost { get; set; } = string.Empty;
        public string last_24_hours { get; set; } = string.Empty;
        public string sold { get; private set; } = string.Empty;
        public string delivery { get; set; } = string.Empty;
        public string return_period { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public string shipping { get; set; } = string.Empty;
        public string seller { get; set;} = string.Empty;
        public string feedback_profile { get; set;} = string.Empty;
        public string store { get; set; } = string.Empty;


    }
}
