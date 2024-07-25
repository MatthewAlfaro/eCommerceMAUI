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
    public class ShopViewModel : INotifyPropertyChanged
    {
        private readonly ShoppingCartService _shoppingCartService;
        public ObservableCollection<ProductViewModel> Products { get; private set; }
        public ObservableCollection<ProductViewModel> CartItems { get; private set; }
        public ObservableCollection<Wishlist> Wishlists { get; private set; }

        public decimal TotalPrice => _shoppingCartService.TotalPrice * (1 + AppSettings.TaxRate);

        public ICommand RemoveFromCartCommand { get; private set; }
        public ICommand CheckoutCommand { get; private set; }
        public ICommand LoadWishlistCommand { get; private set; }

        public ShopViewModel()
        {
            _shoppingCartService = ShoppingCartService.Current;
            Products = new ObservableCollection<ProductViewModel>();
            CartItems = new ObservableCollection<ProductViewModel>();
            Wishlists = _shoppingCartService.Wishlists;

            RemoveFromCartCommand = new Command<ProductViewModel>(RemoveFromCart);
            CheckoutCommand = new Command(Checkout);
            LoadWishlistCommand = new Command<Wishlist>(LoadWishlist);

            LoadProducts();
        }

        private void LoadProducts()
        {
            var products = InventoryService.Current.Products;
            Products.Clear();
            foreach (var product in products)
            {
                Products.Add(new ProductViewModel(product));
            }
        }

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

        public void RemoveFromCart(ProductViewModel productViewModel)
        {
            var product = productViewModel.Model;
            _shoppingCartService.RemoveFromCart(product);
            UpdateCartItems();
        }

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

        public void LoadWishlist(Wishlist wishlist)
        {
            _shoppingCartService.ClearCart();
            foreach (var product in wishlist.Products)
            {
                var finalPrice = product.Price * (1 - (product.MarkdownPercentage / 100m));
                _shoppingCartService.AddToCart(product, product.Quantity, finalPrice);
            }
            UpdateCartItems();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
