using Amazon.Library.Models;
using System.Collections.Generic;

namespace Amazon.Library.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Product> Contents { get; set; }
        public decimal TotalPrice { get; set; }

        public ShoppingCart()
        {
            Contents = new List<Product>();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
