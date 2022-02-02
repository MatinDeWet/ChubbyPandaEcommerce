using Microsoft.EntityFrameworkCore;

namespace ChubbyPandaEcommerce.Server.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                    new Product
                    {
                        Id = 1,
                        Title = "Figur",
                        Description = "Figurine anime",
                        ImageUrl = "https://www.ubuy.za.com/productimg/?image=aHR0cHM6Ly9tLm1lZGlhLWFtYXpvbi5jb20vaW1hZ2VzL0kvNzFZcjErbXRDMkwuX0FDX1NMMTUwMF8uanBn.jpg",
                        Price = 9.99m
                    },
                    new Product
                    {
                        Id = 2,
                        Title = "Tanya",
                        Description = "Figurine anime",
                        ImageUrl = "https://m.media-amazon.com/images/I/61CXYX0Qy2L._AC_SY679_.jpg",
                        Price = 9.99m
                    }
                );
        }

        public DbSet<Product> Products { get; set; }
    }
}
