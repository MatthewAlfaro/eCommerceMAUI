using System.ComponentModel;
using System.Runtime.CompilerServices;
using eCommerce.MAUI.Utilities;

namespace eCommerce.MAUI.ViewModels
{
    public class ConfigurationViewModel : INotifyPropertyChanged
    {
        private decimal _taxRate;

        public decimal TaxRate
        {
            get => _taxRate;
            set
            {
                _taxRate = value;
                NotifyPropertyChanged();
            }
        }

        public ConfigurationViewModel()
        {
            _taxRate = AppSettings.TaxRate;
        }

        public void SaveSettings()
        {
            AppSettings.TaxRate = _taxRate;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
