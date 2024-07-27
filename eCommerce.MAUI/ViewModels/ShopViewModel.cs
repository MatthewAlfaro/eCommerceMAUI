using Amazon.Library.Models;
using Amazon.Library.Services;
using eCommerce.MAUI.Utilities;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace eCommerce.MAUI.ViewModels
{
    // ViewModel for managing the shop view
    public class ShopViewModel : INotifyPropertyChanged
    {
        private readonly ShoppingCartService _shoppingCartService;
        public ObservableCollection<ProductViewModel> Products { get; private set; } 
        public ObservableCollection<ProductViewModel> CartItems { get; private set; } 
        public ObservableCollection<ShoppingCart> ShoppingCarts { get; private set; } 

        // Total price of items in the cart including tax
        public decimal TotalPrice => _shoppingCartService.TotalPrice * (1 + AppSettings.TaxRate);

        // Commands for UI interaction
        public ICommand RemoveFromCartCommand { get; private set; }
        public ICommand CheckoutCommand { get; private set; }
        public ICommand LoadShoppingCartCommand { get; private set; }

        public ShopViewModel()
        {
            // Initialize services and collections
            _shoppingCartService = ShoppingCartService.Current;
            Products = new ObservableCollection<ProductViewModel>();
            CartItems = new ObservableCollection<ProductViewModel>();
            ShoppingCarts = _shoppingCartService.ShoppingCarts;

            // Initialize commands
            RemoveFromCartCommand = new Command<ProductViewModel>(RemoveFromCart);
            CheckoutCommand = new Command(Checkout);
            LoadShoppingCartCommand = new Command<ShoppingCart>(LoadShoppingCart);

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

            if (product.Quantity < quantity)
            {
                return;
            }

            _shoppingCartService.AddToCart(product, quantity, finalPrice);

            UpdateCartItems();
        }

        // Remove a product from the cart
        public void RemoveFromCart(ProductViewModel productViewModel)
        {
            var product = productViewModel.Model;
            _shoppingCartService.RemoveFromCart(product);
            UpdateCartItems();
        }

        // Update the list of items in the cart and the total price
        private void UpdateCartItems()
        {
            CartItems.Clear();
            foreach (var item in _shoppingCartService.Cart)
            {
                CartItems.Add(new ProductViewModel(item));
            }
            NotifyPropertyChanged(nameof(CartItems));
            NotifyPropertyChanged(nameof(TotalPrice));
        }

        // Checkout process: update inventory and clear the cart
        public void Checkout()
        {
            foreach (var item in _shoppingCartService.Cart.ToList())
            {
                var product = InventoryService.Current.Products.FirstOrDefault(p => p.Id == item.Id);
                if (product != null)
                {
                    int quantityToDeduct = product.IsBuyOneGetOneFree ? item.Quantity * 2 : item.Quantity;
                    product.Quantity -= quantityToDeduct;
                    InventoryService.Current.AddOrUpdate(product);
                }
            }

            _shoppingCartService.Checkout();
            UpdateCartItems();
            LoadProducts();
        }

        // Load a saved shopping cart
        public void LoadShoppingCart(ShoppingCart shoppingCart)
        {
            _shoppingCartService.ClearCart();
            foreach (var product in shoppingCart.Contents)
            {
                var finalPrice = product.Price * (1 - (product.MarkdownPercentage / 100m));
                _shoppingCartService.AddToCart(product, product.Quantity, finalPrice);
            }
            UpdateCartItems();
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
