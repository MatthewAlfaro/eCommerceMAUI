using Amazon.Library.Models;
using Amazon.Library.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace eCommerce.MAUI.ViewModels
{
    // ViewModel for managing a single product
    public class ProductViewModel : INotifyPropertyChanged
    {
        // Property to store the product model
        public Product Model { get; set; }

        // Property to display the product price in a formatted string
        public string DisplayPrice => Model == null ? string.Empty : $"{Model.Price:C}";

        // Property to get/set the price as a string
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

        // Property to get/set the product quantity
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

        // Property to get/set the BOGO status
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

        // Property to get/set the markdown percentage
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

        // Default constructor initializes a new Product model
        public ProductViewModel()
        {
            Model = new Product();
        }

        // Constructor that accepts a Product model
        public ProductViewModel(Product model)
        {
            Model = model ?? new Product();
        }

        // Method to add or update the product in the inventory
        public void AddOrUpdate()
        {
            if (Model != null)
            {
                InventoryService.Current.AddOrUpdate(Model);
            }
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
