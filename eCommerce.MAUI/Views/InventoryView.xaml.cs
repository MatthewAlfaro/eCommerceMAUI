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
            Shell.Current.GoToAsync("//Product");
        }

        private void CancelClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//MainPage");
        }

        private void EditClicked(object sender, EventArgs e)
        {
            var productViewModel = (sender as Button)?.CommandParameter as ProductViewModel;
            (BindingContext as InventoryViewModel)?.Edit(productViewModel);
        }

        private void DeleteClicked(object sender, EventArgs e)
        {
            var productViewModel = (sender as Button)?.CommandParameter as ProductViewModel;
            (BindingContext as InventoryViewModel)?.Delete(productViewModel);
        }

        private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
        {
            (BindingContext as InventoryViewModel)?.Refresh();
        }
    }
}