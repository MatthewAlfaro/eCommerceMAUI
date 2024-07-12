using eCommerce.MAUI.ViewModels;
using Microsoft.Maui.Controls;

namespace eCommerce.MAUI.Views
{
    public partial class ConfigurationView : ContentPage
    {
        public ConfigurationView()
        {
            InitializeComponent();
            BindingContext = new ConfigurationViewModel();
        }

        private void SaveClicked(object sender, EventArgs e)
        {
            var configViewModel = BindingContext as ConfigurationViewModel;
            configViewModel?.SaveSettings();
            Shell.Current.GoToAsync("//MainPage");
        }
    }
}
