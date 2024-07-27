using System.ComponentModel;
using System.Runtime.CompilerServices;
using eCommerce.MAUI.Utilities;

namespace eCommerce.MAUI.ViewModels
{
    // ViewModel for managing application configuration settings
    public class ConfigurationViewModel : INotifyPropertyChanged
    {
        // Private field for storing the tax rate
        private decimal _taxRate;

        // Property for accessing and modifying the tax rate
        public decimal TaxRate
        {
            get => _taxRate; 
            set
            {
                _taxRate = value; 
                NotifyPropertyChanged(); 
            }
        }

        // Constructor initializes the tax rate from application settings
        public ConfigurationViewModel()
        {
            _taxRate = AppSettings.TaxRate;
        }

        // Method to save the tax rate to application settings
        public void SaveSettings()
        {
            AppSettings.TaxRate = _taxRate;
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
