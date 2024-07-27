using Amazon.Library.Models;
using eCommerce.MAUI.ViewModels;
using Microsoft.Maui.Controls;

namespace eCommerce.MAUI.Views
{
    public partial class ShopView : ContentPage
    {
        public ShopView()
        {
            InitializeComponent();
            BindingContext = new ShopViewModel();
        }

        private void CancelClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//MainPage");
        }

        private void AddToCartClicked(object sender, EventArgs e)
        {
            // Add product to the cart
            var button = sender as Button;
            var productViewModel = button?.CommandParameter as ProductViewModel;

            if (productViewModel != null)
            {
                var entry = button?.Parent?.FindByName<Entry>("quantityEntry");
                if (entry != null && int.TryParse(entry.Text, out int quantity))
                {
                    (BindingContext as ShopViewModel)?.AddToCart(productViewModel, quantity);

                    entry.Text = string.Empty;
                }
            }
        }

        private void LoadShoppingCartClicked(object sender, EventArgs e)
        {
            // Load the selected shopping cart
            var button = sender as Button;
            var shoppingCart = button?.CommandParameter as ShoppingCart;

            if (shoppingCart != null)
            {
                (BindingContext as ShopViewModel)?.LoadShoppingCart(shoppingCart);
            }
        }
    }
}
