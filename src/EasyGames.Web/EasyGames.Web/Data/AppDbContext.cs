using EasyGames.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace EasyGames.Web.Data
{
<<<<<<< HEAD
=======
    // student-style: EF Core context with our tables
>>>>>>> feature/akshata/data-shops
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products => Set<Product>();
<<<<<<< HEAD
        public DbSet<ShopLocation> Shops => Set<ShopLocation>();  // ← CHANGED
=======
        public DbSet<Shop> Shops => Set<Shop>();
>>>>>>> feature/akshata/data-shops
        public DbSet<ShopStock> ShopStocks => Set<ShopStock>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderLine> OrderLines => Set<OrderLine>();
        public DbSet<AppUser> AppUsers => Set<AppUser>();
<<<<<<< HEAD
        public DbSet<OwnerStock> OwnerStocks => Set<OwnerStock>();
    }
}
=======

        public DbSet<OwnerStock> OwnerStocks => Set<OwnerStock>();


    }
}
>>>>>>> feature/akshata/data-shops
