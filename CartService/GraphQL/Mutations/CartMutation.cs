using CartService.Data;
using CartService.Models;
using ProductService.Models;
using System.Text.Json;

namespace CartService.GraphQL.Mutations
{
    public class CartMutation
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public CartMutation(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        // Add an item to the cart
        public async Task<CartItem> AddToCart(CartItem input, [Service] CartDbContext context)
        {
            //Validate ProductId with ProductService
            var client = _httpClientFactory.CreateClient("ProductService");
            var response = await client.GetAsync($"products/{input.ProductId}");
            if (!response.IsSuccessStatusCode)
                throw new Exception("Invalid ProductId");
            var product = JsonSerializer.Deserialize<Product>(await response.Content.ReadAsStringAsync());
            if (product == null) throw new Exception("Product not found");
            input.Price = product.Price; //Sync price with ProductService
            context.CartItems.Add(input);
            await context.SaveChangesAsync();
            return input;
        }
        // Update an item in the cart
        public async Task<CartItem> UpdateCartItem(int id, CartItem input, [Service] CartDbContext context)
        {
            var item = await context.CartItems.FindAsync(id);
            if (item == null) throw new Exception("Cart item not found");

            item.Quantity = input.Quantity;
            item.Price = input.Price;
            await context.SaveChangesAsync();
            return item;
        }
        // Remove an item from the cart
        public async Task<bool> RemoveFromCart(int id, [Service] CartDbContext context)
        {
            var item = await context.CartItems.FindAsync(id);
            if (item == null) return false;

            context.CartItems.Remove(item);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
