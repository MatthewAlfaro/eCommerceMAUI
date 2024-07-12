using Amazon.Library.Models;
using Amazon.Library.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace eCommerce.MAUI.ViewModels
{
    public class WishlistViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ProductViewModel> Products { get; private set; }
        public ObservableCollection<ProductViewModel> CartItems { get; private set; }
        public decimal TotalPrice => ShoppingCartService.Current.TotalPrice;

        public WishlistViewModel()
        {
            Products = new ObservableCollection<ProductViewModel>();
            CartItems = new ObservableCollection<ProductViewModel>();
            LoadProducts();
        }

        private void LoadProducts()
        {
            var products = InventoryServiceProxy.Current.Products;
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

            ShoppingCartService.Current.AddToCart(product, quantity, finalPrice);
            UpdateCartItems();
        }

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

        public void SaveWishlist(string name)
        {
            ShoppingCartService.Current.SaveWishlist(name);
            UpdateCartItems(); 
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
