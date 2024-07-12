using Amazon.Library.Models;
using Amazon.Library.Services;
using eCommerce.MAUI.Utilities;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace eCommerce.MAUI.ViewModels
{
    public class ShopViewModel : INotifyPropertyChanged
    {
        private readonly ShoppingCartService _shoppingCartService;
        public ObservableCollection<ProductViewModel> Products { get; private set; }
        public ObservableCollection<ProductViewModel> CartItems { get; private set; }
        public ObservableCollection<Wishlist> Wishlists { get; private set; }

        public decimal TotalPrice
        {
            get => _shoppingCartService.TotalPrice * (1 + AppSettings.TaxRate);
        }

        public ShopViewModel()
        {
            _shoppingCartService = ShoppingCartService.Current;
            Products = new ObservableCollection<ProductViewModel>();
            CartItems = new ObservableCollection<ProductViewModel>();
            Wishlists = _shoppingCartService.Wishlists;

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

            if (product.Quantity < quantity)
            {
                return;
            }

            product.Quantity -= quantity;

            InventoryServiceProxy.Current.AddOrUpdate(product);

            _shoppingCartService.AddToCart(product, quantity, finalPrice);

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

        private int CalculateEffectiveQuantity(Product product, int quantity)
        {
            return product.IsBuyOneGetOneFree ? quantity * 2 : quantity;
        }

        public void Checkout()
        {
            foreach (var item in _shoppingCartService.Cart.ToList()) 
            {
                var product = InventoryServiceProxy.Current.Products.FirstOrDefault(p => p.Id == item.Id);
                if (product != null)
                {
                    int quantityToDeduct = CalculateEffectiveQuantity(item, item.Quantity);
                    product.Quantity -= quantityToDeduct;
                    InventoryServiceProxy.Current.AddOrUpdate(product);
                }
            }

            _shoppingCartService.Checkout();
            UpdateCartItems();
            LoadProducts();
        }

        public void LoadWishlist(Wishlist wishlist)
        {
            _shoppingCartService.LoadWishlist(wishlist);
            UpdateCartItems();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
