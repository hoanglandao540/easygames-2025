using EasyGames.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace EasyGames.Web.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<ShopLocation> Shops => Set<ShopLocation>();  // ← CHANGED
        public DbSet<ShopStock> ShopStocks => Set<ShopStock>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderLine> OrderLines => Set<OrderLine>();
        public DbSet<AppUser> AppUsers => Set<AppUser>();
        public DbSet<OwnerStock> OwnerStocks => Set<OwnerStock>();
    }
}