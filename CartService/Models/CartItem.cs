namespace CartService.Models
{
    public class CartItem
    {
        public int Id { get; set; } // Primary Key
        public int ProductId { get; set; } // ID of the product
        public string ProductName { get; set; } = string.Empty; // Name of the product
        public decimal Price { get; set; } // Price of the product
        public int Quantity { get; set; } // Quantity in the cart
        public decimal Total => Price * Quantity; // Computed Total
    }
}
