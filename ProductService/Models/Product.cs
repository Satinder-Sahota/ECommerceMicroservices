namespace ProductService.Models
{
    public class Product
    {
        public int Id { get; set; } // Primary Key
        public string Name { get; set; } = string.Empty; // Product name
        public string Description { get; set; } = string.Empty; // Product description
        public decimal Price { get; set; } // Product price
        public int Stock { get; set; } // Available stock
    }
}
