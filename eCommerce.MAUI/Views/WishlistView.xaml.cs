using Amazon.Library.Models;
using eCommerce.MAUI.ViewModels;
using Microsoft.Maui.Controls;

namespace eCommerce.MAUI.Views
{
    public partial class WishlistView : ContentPage
    {
        public WishlistView()
        {
            InitializeComponent();
            BindingContext = new WishlistViewModel();
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
                    (BindingContext as WishlistViewModel)?.AddToCart(productViewModel, quantity);

                    entry.Text = string.Empty;
                }
            }
        }

        private void SaveWishlistClicked(object sender, EventArgs e)
        {
            // Save the current cart as a wishlist
            var wishlistName = wishlistNameEntry.Text;
            if (!string.IsNullOrEmpty(wishlistName))
            {
                (BindingContext as WishlistViewModel)?.SaveShoppingCart(wishlistName);
            }
        }

        private void LoadShoppingCartClicked(object sender, EventArgs e)
        {
            // Load the selected shopping cart
            var button = sender as Button;
            var shoppingCart = button?.CommandParameter as ShoppingCart;
            var viewModel = BindingContext as WishlistViewModel;
            viewModel?.LoadShoppingCart(shoppingCart);
        }

        private void DeleteShoppingCartClicked(object sender, EventArgs e)
        {
            // Delete the selected shopping cart
            var button = sender as Button;
            var shoppingCart = button?.CommandParameter as ShoppingCart;
            var viewModel = BindingContext as WishlistViewModel;
            viewModel?.DeleteShoppingCart(shoppingCart);
        }

        private void CancelClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//MainPage");
        }
    }
}
