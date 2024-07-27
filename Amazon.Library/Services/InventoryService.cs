using Amazon.Library.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Amazon.Library.Services
{
    public class InventoryService
    {
        // Singleton instance
        private static InventoryService? instance;
        private static readonly object instanceLock = new object();

        // List of products in the inventory
        private List<Product> products;

        // Notify when inventory is updated
        public event Action InventoryUpdated;

        // Get the products as a read-only collection
        public ReadOnlyCollection<Product> Products
        {
            get
            {
                return products.AsReadOnly();
            }
        }

        // Get the next product ID
        private int NextId
        {
            get
            {
                if (!products.Any())
                {
                    return 1;
                }

                return products.Select(p => p.Id).Max() + 1;
            }
        }

        // Method to add or update a product in the inventory
        public Product AddOrUpdate(Product p)
        {
            bool isAdd = false;
            if (p.Id == 0)
            {
                isAdd = true;
                p.Id = NextId; 
            }

            if (isAdd)
            {
                products.Add(p); 
            }
            else
            {
                var existingProduct = products.FirstOrDefault(prod => prod.Id == p.Id);
                if (existingProduct != null)
                {
                    // Update existing product attributes
                    existingProduct.Name = p.Name;
                    existingProduct.Price = p.Price;
                    existingProduct.Quantity = p.Quantity;
                    existingProduct.MarkdownPercentage = p.MarkdownPercentage;
                }
            }

            InventoryUpdated?.Invoke();

            return p;
        }

        // Method to delete a product from the inventory
        public void Delete(Product p)
        {
            products.RemoveAll(prod => prod.Id == p.Id);
            InventoryUpdated?.Invoke(); // Invoke the inventory updated event
        }

        // Constructor to initialize the inventory with sample data
        private InventoryService()
        {
            // Sample data for testing
            products = new List<Product>{
                new Product{Id = 1,Name = "Peanut Butter", Price=5.00M, Quantity=200},
                new Product{Id = 2,Name = "Jelly", Price=20.00M, Quantity=100},
                new Product{Id = 3,Name = "Bread", Price= 100.00M, Quantity=800}
            };
        }

        // Get the singleton instance of the InventoryService
        public static InventoryService Current
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new InventoryService();
                    }
                }
                return instance;
            }
        }
    }
}
