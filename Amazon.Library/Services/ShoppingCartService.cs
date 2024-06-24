using Amazon.Library.Models;
using System.Collections.ObjectModel;
using System.Linq;

public class ShoppingCartService
{
    public static ShoppingCartService Current { get; } = new ShoppingCartService();

    public ObservableCollection<Product> Cart { get; set; }
    public decimal TotalPrice { get; set; }

    private ShoppingCartService()
    {
        Cart = new ObservableCollection<Product>();
        TotalPrice = 0;
    }

    public void AddToCart(Product product, int quantity)
    {
        var existingProduct = Cart.FirstOrDefault(p => p.Id == product.Id);
        if (existingProduct != null)
        {
            existingProduct.Quantity += quantity;
        }
        else
        {
            Cart.Add(new Product
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Quantity = quantity
            });
        }

        TotalPrice += product.Price * quantity;
    }

    public void RemoveFromCart(Product product)
    {
        Cart.Remove(product);
        TotalPrice -= product.Price * product.Quantity;
    }

    public void ClearCart()
    {
        Cart.Clear();
        TotalPrice = 0;
    }
}
