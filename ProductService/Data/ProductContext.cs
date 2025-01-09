using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using ProductService.Models;

namespace ProductService.Data
{
    public class ProductContext : DbContext
    {

        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        { }
        public DbSet<Product> Products { get; set; } // Table for products
    }
} 
    
