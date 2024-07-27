using Amazon.Library.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Amazon.Library.Services
{
    public class ShoppingCartService
    {
        // Singleton instance
        public static ShoppingCartService Current { get; } = new ShoppingCartService();

        // Observable collection for the cart items
        public ObservableCollection<Product> Cart { get; private set; }

        // Store the total price
        private decimal totalPrice;

        // Property to get the total price of the cart
        public decimal TotalPrice
        {
            get => totalPrice;
            private set => totalPrice = value;
        }

        // Observable collection for the saved shopping carts
        public ObservableCollection<ShoppingCart> ShoppingCarts { get; private set; }

        // Constructor to initialize the cart and shopping carts
        private ShoppingCartService()
        {
            Cart = new ObservableCollection<Product>();
            ShoppingCarts = new ObservableCollection<ShoppingCart>();
            TotalPrice = 0;
        }

        // Method to add a product to the cart
        public void AddToCart(Product product, int quantity, decimal finalPrice)
        {
            var existingProduct = Cart.FirstOrDefault(p => p.Id == product.Id);
            if (existingProduct != null)
            {
                // If product exists in the cart, update the quantity
                existingProduct.Quantity += quantity;
            }
            else
            {
                // If product is new, add it to the cart
                Cart.Add(new Product
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = finalPrice,
                    Quantity = quantity,
                });
            }

            TotalPrice += finalPrice * quantity;
        }

        // Method to remove a product from the cart
        public void RemoveFromCart(Product product)
        {
            var existingProduct = Cart.FirstOrDefault(p => p.Id == product.Id);
            if (existingProduct != null)
            {
                // Update the total price and remove the product from the cart
                TotalPrice -= existingProduct.Price * existingProduct.Quantity;
                Cart.Remove(existingProduct);
            }
        }

        // Method to clear the cart
        public void ClearCart()
        {
            Cart.Clear();
            TotalPrice = 0;
        }

        // Method to save the current cart as a named shopping cart
        public void SaveShoppingCart(string name)
        {
            var existingCart = ShoppingCarts.FirstOrDefault(c => c.Name == name);
            if (existingCart != null)
            {
                // If a cart with the same name exists, update the contents and total price
                existingCart.Contents = new List<Product>(Cart);
                existingCart.TotalPrice = TotalPrice;
            }
            else
            {
                // If it's a new cart, create and add it to the collection
                var shoppingCart = new ShoppingCart
                {
                    Name = name,
                    Contents = new List<Product>(Cart),
                    TotalPrice = TotalPrice
                };
                ShoppingCarts.Add(shoppingCart);
            }

            ClearCart(); 
        }

        // Method to load a saved shopping cart into the current cart
        public void LoadShoppingCart(ShoppingCart shoppingCart)
        {
            ClearCart(); 
            foreach (var product in shoppingCart.Contents)
            {
                AddToCart(product, product.Quantity, product.Price);
            }
        }

        // Method to delete a saved shopping cart
        public void DeleteShoppingCart(ShoppingCart shoppingCart)
        {
            ShoppingCarts.Remove(shoppingCart);
        }

        // Method to checkout the current cart
        public void Checkout()
        {
            Cart.Clear(); 
            TotalPrice = 0;
        }
    }
}
