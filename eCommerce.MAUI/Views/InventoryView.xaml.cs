using eCommerce.MAUI.ViewModels;
using Microsoft.Maui.Controls;

namespace eCommerce.MAUI.Views
{
    public partial class InventoryView : ContentPage
    {
        public InventoryView()
        {
            InitializeComponent();
            BindingContext = new InventoryViewModel();
        }

        private void AddClicked(object sender, EventArgs e)
        {
            // Navigate to ProductView with a new ProductViewModel
            var newProductViewModel = new ProductViewModel(new Amazon.Library.Models.Product());
            var productView = new ProductView();
            productView.BindingContext = newProductViewModel;
            Shell.Current.Navigation.PushAsync(productView);
        }

        private void CancelClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//MainPage");
        }

        private void EditClicked(object sender, EventArgs e)
        {
            // Navigate to ProductView with the selected ProductViewModel
            var productViewModel = (sender as Button)?.CommandParameter as ProductViewModel;
            (BindingContext as InventoryViewModel)?.Edit(productViewModel);
        }

        private void DeleteClicked(object sender, EventArgs e)
        {
            // Delete the selected ProductViewModel
            var productViewModel = (sender as Button)?.CommandParameter as ProductViewModel;
            (BindingContext as InventoryViewModel)?.Delete(productViewModel);
        }

        private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
        {
            // Refresh the product list when navigated to this page
            (BindingContext as InventoryViewModel)?.Refresh();
        }
    }
}
