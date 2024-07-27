using Amazon.Library.Models;
using Amazon.Library.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace eCommerce.MAUI.ViewModels
{
    // ViewModel for managing the wishlist view
    public class WishlistViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ProductViewModel> Products { get; private set; } 
        public ObservableCollection<ProductViewModel> CartItems { get; private set; } 
        public ObservableCollection<ShoppingCart> ShoppingCarts => ShoppingCartService.Current.ShoppingCarts; 
        public decimal TotalPrice => ShoppingCartService.Current.TotalPrice; 

        // Commands for UI interaction
        public ICommand LoadShoppingCartCommand { get; private set; }
        public ICommand DeleteShoppingCartCommand { get; private set; }
        public ICommand RemoveFromCartCommand { get; private set; }

        public WishlistViewModel()
        {
            Products = new ObservableCollection<ProductViewModel>();
            CartItems = new ObservableCollection<ProductViewModel>();
            LoadShoppingCartCommand = new Command<ShoppingCart>(LoadShoppingCart);
            DeleteShoppingCartCommand = new Command<ShoppingCart>(DeleteShoppingCart);
            RemoveFromCartCommand = new Command<ProductViewModel>(RemoveFromCart);

            // Inventory updates
            InventoryService.Current.InventoryUpdated += OnInventoryUpdated;
            LoadProducts();
        }

        // Event handler for inventory updates
        private void OnInventoryUpdated()
        {
            LoadProducts();
        }

        // Load products from the inventory service
        private void LoadProducts()
        {
            var products = InventoryService.Current.Products;
            Products.Clear();
            foreach (var product in products)
            {
                Products.Add(new ProductViewModel(product));
            }
        }

        // Add a product to the cart
        public void AddToCart(ProductViewModel productViewModel, int quantity)
        {
            var product = productViewModel.Model;
            var finalPrice = product.Price * (1 - (product.MarkdownPercentage / 100m));
            ShoppingCartService.Current.AddToCart(product, quantity, finalPrice);
            UpdateCartItems();
        }

        // Remove a product from the cart
        public void RemoveFromCart(ProductViewModel productViewModel)
        {
            var product = productViewModel.Model;
            ShoppingCartService.Current.RemoveFromCart(product);
            UpdateCartItems();
        }

        // Update the list of items in the cart and the total price
        private void UpdateCartItems()
        {
            CartItems.Clear();
            foreach (var item in ShoppingCartService.Current.Cart)
            {
                CartItems.Add(new ProductViewModel(item));
            }
            NotifyPropertyChanged(nameof(CartItems));
            NotifyPropertyChanged(nameof(TotalPrice));
        }

        // Save the current cart as a shopping cart
        public void SaveShoppingCart(string name)
        {
            ShoppingCartService.Current.SaveShoppingCart(name);
            UpdateCartItems();
            NotifyPropertyChanged(nameof(ShoppingCarts));
        }

        // Load a saved shopping cart
        public void LoadShoppingCart(ShoppingCart shoppingCart)
        {
            ShoppingCartService.Current.LoadShoppingCart(shoppingCart);
            UpdateCartItems();
        }

        // Delete a saved shopping cart
        public void DeleteShoppingCart(ShoppingCart shoppingCart)
        {
            ShoppingCartService.Current.DeleteShoppingCart(shoppingCart);
            NotifyPropertyChanged(nameof(ShoppingCarts));
        }

        // Notifying the UI of property changes
        public event PropertyChangedEventHandler PropertyChanged;

        // Method to invoke the PropertyChanged event
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
