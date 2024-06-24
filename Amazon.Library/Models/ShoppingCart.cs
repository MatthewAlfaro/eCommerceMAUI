using Amazon.Library.Models;
using System.Collections.Generic;

namespace Amazon.Library.Models
{
    public class ShoppingCart
    {
        public List<Product> Contents { get; set; }

        public ShoppingCart()
        {
            Contents = new List<Product>();
        }
    }
}
