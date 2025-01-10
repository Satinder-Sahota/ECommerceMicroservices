using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using ProductService.Models;

namespace ProductService.Data
{
    public class ProductDbContext : DbContext
    {

        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
        { }
        public DbSet<Product> Products { get; set; } // Table for products
    }
} 
    
