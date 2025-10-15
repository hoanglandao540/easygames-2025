using EasyGames.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace EasyGames.Web.Data
{
    // student-style: EF Core context with our tables
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Shop> Shops => Set<Shop>();
        public DbSet<ShopStock> ShopStocks => Set<ShopStock>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderLine> OrderLines => Set<OrderLine>();
        public DbSet<AppUser> AppUsers => Set<AppUser>();

    }
}
