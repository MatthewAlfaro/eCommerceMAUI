﻿using Amazon.Library.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Amazon.Library.Services
{
    public class ShoppingCartService
    {
        public static ShoppingCartService Current { get; } = new ShoppingCartService();

        public ObservableCollection<Product> Cart { get; private set; }
        private decimal totalPrice;
        public decimal TotalPrice
        {
            get => totalPrice;
            private set => totalPrice = value;
        }

        public ObservableCollection<Wishlist> Wishlists { get; private set; }

        private ShoppingCartService()
        {
            Cart = new ObservableCollection<Product>();
            Wishlists = new ObservableCollection<Wishlist>();
            TotalPrice = 0;
        }

        public void AddToCart(Product product, int quantity, decimal finalPrice)
        {
            var existingProduct = Cart.FirstOrDefault(p => p.Id == product.Id);
            if (existingProduct != null)
            {
                existingProduct.Quantity += quantity;
            }
            else
            {
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

        public void RemoveFromCart(Product product)
        {
            var existingProduct = Cart.FirstOrDefault(p => p.Id == product.Id);
            if (existingProduct != null)
            {
                TotalPrice -= existingProduct.Price * existingProduct.Quantity;
                Cart.Remove(existingProduct);
            }
        }

        public void ClearCart()
        {
            Cart.Clear();
            TotalPrice = 0;
        }

        public void SaveWishlist(string name)
        {
            var existingWishlist = Wishlists.FirstOrDefault(w => w.Name == name);
            if (existingWishlist != null)
            {
                existingWishlist.Products = new List<Product>(Cart);
                existingWishlist.TotalPrice = TotalPrice;
            }
            else
            {
                var wishlist = new Wishlist
                {
                    Name = name,
                    Products = new List<Product>(Cart),
                    TotalPrice = TotalPrice
                };
                Wishlists.Add(wishlist);
            }

            ClearCart();
        }

        public void LoadWishlist(Wishlist wishlist)
        {
            ClearCart();
            foreach (var product in wishlist.Products)
            {
                AddToCart(product, product.Quantity, product.Price);
            }
        }

        public void DeleteWishlist(Wishlist wishlist)
        {
            Wishlists.Remove(wishlist);
        }

        public void Checkout()
        {
            Cart.Clear();
            TotalPrice = 0;
        }
    }
}
