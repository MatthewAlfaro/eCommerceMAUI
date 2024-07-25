using Amazon.Library.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Amazon.Library.Models;

namespace eCommerce.MAUI.ViewModels
{
    public class WishlistViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ProductViewModel> Products { get; private set; }
        public ObservableCollection<ProductViewModel> CartItems { get; private set; }
        public ObservableCollection<Wishlist> Wishlists => ShoppingCartService.Current.Wishlists;
        public decimal TotalPrice => ShoppingCartService.Current.TotalPrice;

        public ICommand LoadWishlistCommand { get; private set; }
        public ICommand DeleteWishlistCommand { get; private set; }
        public ICommand RemoveFromCartCommand { get; private set; }

        public WishlistViewModel()
        {
            Products = new ObservableCollection<ProductViewModel>();
            CartItems = new ObservableCollection<ProductViewModel>();
            LoadWishlistCommand = new Command<Wishlist>(LoadWishlist);
            DeleteWishlistCommand = new Command<Wishlist>(DeleteWishlist);
            RemoveFromCartCommand = new Command<ProductViewModel>(RemoveFromCart);

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
            ShoppingCartService.Current.AddToCart(product, quantity, finalPrice);
            UpdateCartItems();
        }

        public void RemoveFromCart(ProductViewModel productViewModel)
        {
            var product = productViewModel.Model;
            ShoppingCartService.Current.RemoveFromCart(product);
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
            NotifyPropertyChanged(nameof(Wishlists));
        }

        public void LoadWishlist(Wishlist wishlist)
        {
            ShoppingCartService.Current.LoadWishlist(wishlist);
            UpdateCartItems();
        }

        public void DeleteWishlist(Wishlist wishlist)
        {
            ShoppingCartService.Current.DeleteWishlist(wishlist);
            NotifyPropertyChanged(nameof(Wishlists));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
