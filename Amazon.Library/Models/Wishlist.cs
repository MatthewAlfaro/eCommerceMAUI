using Amazon.Library.Models;
using System.Collections.Generic;

namespace Amazon.Library.Models
{
    public class Wishlist
    {
        public string Name { get; set; }
        public List<Product> Products { get; set; }
        public decimal TotalPrice { get; set; }

        public Wishlist()
        {
            Products = new List<Product>();
        }
    }
}
