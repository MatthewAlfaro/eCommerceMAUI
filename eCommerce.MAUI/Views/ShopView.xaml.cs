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
            var button = sender as Button;
            var productViewModel = button?.CommandParameter as ProductViewModel;

            if (productViewModel != null)
            {
                var entry = button?.Parent?.FindByName<Entry>("quantityEntry");
                if (entry != null && int.TryParse(entry.Text, out int quantity))
                {
                    (BindingContext as ShopViewModel)?.AddToCart(productViewModel, quantity);

                    // Clear the quantity entry after adding to cart
                    entry.Text = string.Empty;
                }
            }
        }
        private void LoadWishlistClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var wishlist = button?.CommandParameter as Wishlist;

            if (wishlist != null)
            {
                (BindingContext as ShopViewModel)?.LoadWishlist(wishlist);
            }
        }
    }
}

