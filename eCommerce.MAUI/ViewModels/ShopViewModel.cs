using Amazon.Library.Models;
using Amazon.Library.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace eCommerce.MAUI.ViewModels
{
    public class ShopViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<ProductViewModel> _products;
        private decimal _totalPrice;

        public ObservableCollection<ProductViewModel> Products
        {
            get => _products;
            set
            {
                _products = value;
                NotifyPropertyChanged();
            }
        }

        public decimal TotalPrice
        {
            get => _totalPrice;
            set
            {
                _totalPrice = value;
                NotifyPropertyChanged();
            }
        }

        public ShopViewModel()
        {
            _products = new ObservableCollection<ProductViewModel>();
            _totalPrice = 0;

            Refresh();
        }

        public void Refresh()
        {
            Products = new ObservableCollection<ProductViewModel>(
                InventoryServiceProxy.Current.Products.Select(p => new ProductViewModel(p))
            );
        }

        public void AddToCart(ProductViewModel productViewModel, int quantity)
        {
            var product = productViewModel.Model;

            if (product.Quantity < quantity)
            {
                // Handle insufficient quantity case
                return;
            }

            // Decrement quantity in inventory
            product.Quantity -= quantity;

            // Update the inventory
            InventoryServiceProxy.Current.AddOrUpdate(product);

            // Add the product to the cart
            ShoppingCartService.Current.AddToCart(product, quantity);

            // Update total price by adding the item price
            TotalPrice = ShoppingCartService.Current.TotalPrice;

            // Refresh the product list to reflect updated quantities
            Refresh();

            // Notify UI of changes
            NotifyPropertyChanged(nameof(Products));
            NotifyPropertyChanged(nameof(TotalPrice));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
