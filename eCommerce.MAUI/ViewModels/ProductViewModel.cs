using Amazon.Library.Models;
using Amazon.Library.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace eCommerce.MAUI.ViewModels
{
    public class ProductViewModel : INotifyPropertyChanged
    {
        public Product Model { get; set; }

        public string DisplayPrice => Model == null ? string.Empty : $"{Model.Price:C}";

        public string PriceAsString
        {
            set
            {
                if (Model != null && decimal.TryParse(value, out var price))
                {
                    Model.Price = price;
                    NotifyPropertyChanged(nameof(DisplayPrice));
                }
            }
        }

        public int Quantity
        {
            get => Model.Quantity;
            set
            {
                if (Model.Quantity != value)
                {
                    Model.Quantity = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool IsBuyOneGetOneFree
        {
            get => Model.IsBuyOneGetOneFree;
            set
            {
                if (Model.IsBuyOneGetOneFree != value)
                {
                    Model.IsBuyOneGetOneFree = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public decimal MarkdownPercentage
        {
            get => Model.MarkdownPercentage;
            set
            {
                if (Model.MarkdownPercentage != value)
                {
                    Model.MarkdownPercentage = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public ProductViewModel()
        {
            Model = new Product();
        }

        public ProductViewModel(Product model)
        {
            Model = model ?? new Product();
        }

        public void AddOrUpdate()
        {
            if (Model != null)
            {
                InventoryService.Current.AddOrUpdate(Model);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
