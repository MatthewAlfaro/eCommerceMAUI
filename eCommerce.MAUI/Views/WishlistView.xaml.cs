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
            var wishlistName = wishlistNameEntry.Text;
            if (!string.IsNullOrEmpty(wishlistName))
            {
                (BindingContext as WishlistViewModel)?.SaveWishlist(wishlistName);
                Shell.Current.GoToAsync("//Shop");
            }
        }

        private void CancelClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//Shop");
        }
    }
}
