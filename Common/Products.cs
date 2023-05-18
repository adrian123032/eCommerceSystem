using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Products
    {
        public string type { get; set; } = string.Empty;
        public int position { get; set; }
        public string name { get; set; } = string.Empty;
        public string image { get; set; } = string.Empty;
        public bool has_prime { get; set; }
        public bool is_best_seller { get; set; }
        public bool is_amazon_choice { get; set; }
        public bool is_limited_deal { get; set; }
        public double stars { get; set; }
        public int total_reviews { get; set; }
        public string url { get; set; } = string.Empty;
        public int availability_quantity { get; set; }
        public string spec { get; set; } = string.Empty;
        public string price_string { get; set; } = string.Empty;
        public string price_symbol { get; set; } = string.Empty;
        public int price { get; set; }
    }
}
