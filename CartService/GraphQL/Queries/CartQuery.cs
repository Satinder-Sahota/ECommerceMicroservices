using CartService.Data;
using CartService.Models;
using HotChocolate.Types;
using HotChocolate.Data;


namespace CartService.GraphQL.Queries
{
    public class CartQuery
    {
        [UseProjection]  // Enables projection for nested queries
        [UseFiltering]   // Enables filtering
        [UseSorting]     // Enables sorting
        public IQueryable<CartItem> GetCartItems([Service] CartDbContext context) 
        {
            // Use the injected DbContext to query the database
            return context.CartItems.AsQueryable();
        }
    }
}
