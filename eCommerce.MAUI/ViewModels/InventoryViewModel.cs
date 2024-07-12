using Amazon.Library.Models;
using Amazon.Library.Services;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using eCommerce.MAUI.Views;

namespace eCommerce.MAUI.ViewModels
{
    public class InventoryViewModel : INotifyPropertyChanged
    {
        private List<ProductViewModel> _products;

        public List<ProductViewModel> Products
        {
            get => _products;
            set
            {
                _products = value;
                NotifyPropertyChanged();
            }
        }

        public InventoryViewModel()
        {
            Refresh();
        }

        public void Refresh()
        {
            Products = InventoryServiceProxy.Current.Products
                .Select(p => new ProductViewModel(p))
                .ToList();
        }

        public void Edit(ProductViewModel productViewModel)
        {
            var productView = new ProductView();
            productView.BindingContext = productViewModel;
            Shell.Current.Navigation.PushAsync(productView);
        }

        public void Delete(ProductViewModel productViewModel)
        {
            var product = productViewModel.Model;
            if (product != null)
            {
                InventoryServiceProxy.Current.Delete(product);
                Refresh(); 
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
