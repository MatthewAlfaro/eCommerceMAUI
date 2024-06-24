using eCommerce.MAUI.ViewModels;
using Microsoft.Maui.Controls;

namespace eCommerce.MAUI.Views
{
    public partial class ProductView : ContentPage
    {
        public ProductView()
        {
            InitializeComponent();
            BindingContext = new ProductViewModel();
        }

        private void CancelClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//Inventory");
        }

        private void OkClicked(object sender, EventArgs e)
        {
            var viewModel = BindingContext as ProductViewModel;
            viewModel?.AddOrUpdate();
            Shell.Current.GoToAsync("//Inventory");
        }
    }
}