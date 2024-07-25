namespace Amazon.Library.Models
{
    public class Product
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Id { get; set; }
        public int Quantity { get; set; }
        public bool IsBuyOneGetOneFree { get; set; } // BOGO property

        public decimal MarkdownPercentage { get; set; } // Markdown property
    }
}
