using Amazon.Library.Models;
using Amazon.Library.Services;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using eCommerce.MAUI.Views;

namespace eCommerce.MAUI.ViewModels
{
    // ViewModel for managing the inventory of products
    public class InventoryViewModel : INotifyPropertyChanged
    {
        // Stores the list of product view models
        private List<ProductViewModel> _products;

        // Property for accessing and modifying the list of product view models
        public List<ProductViewModel> Products
        {
            get => _products; 
            set
            {
                _products = value; 
                NotifyPropertyChanged(); 
            }
        }

        // Constructor initializes the product list by refreshing it
        public InventoryViewModel()
        {
            Refresh();
        }

        // Method to refresh the list of products from the inventory service
        public void Refresh()
        {
            Products = InventoryService.Current.Products
                .Select(p => new ProductViewModel(p))
                .ToList();
        }

        // Method to edit a product
        public void Edit(ProductViewModel productViewModel)
        {
            // Create a new ProductView and set its BindingContext to the productViewModel
            var productView = new ProductView();
            productView.BindingContext = productViewModel;

            Shell.Current.Navigation.PushAsync(productView);
        }

        // Method to delete a product
        public void Delete(ProductViewModel productViewModel)
        {
            var product = productViewModel.Model;
            if (product != null)
            {
                InventoryService.Current.Delete(product);
                Refresh();
            }
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
