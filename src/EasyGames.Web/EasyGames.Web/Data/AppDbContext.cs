using EasyGames.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace EasyGames.Web.Data
{
<<<<<<< HEAD
<<<<<<< HEAD
=======
    // student-style: EF Core context with our tables
>>>>>>> feature/akshata/data-shops
=======
>>>>>>> origin/feature/hoang/pos-tier-email
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products => Set<Product>();
<<<<<<< HEAD
<<<<<<< HEAD
        public DbSet<ShopLocation> Shops => Set<ShopLocation>();  // ← CHANGED
=======
        public DbSet<Shop> Shops => Set<Shop>();
>>>>>>> feature/akshata/data-shops
=======
        public DbSet<ShopLocation> Shops => Set<ShopLocation>();  // ← CHANGED
>>>>>>> origin/feature/hoang/pos-tier-email
        public DbSet<ShopStock> ShopStocks => Set<ShopStock>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderLine> OrderLines => Set<OrderLine>();
        public DbSet<AppUser> AppUsers => Set<AppUser>();
<<<<<<< HEAD
<<<<<<< HEAD
        public DbSet<OwnerStock> OwnerStocks => Set<OwnerStock>();
    }
}
=======

=======
>>>>>>> origin/feature/hoang/pos-tier-email
        public DbSet<OwnerStock> OwnerStocks => Set<OwnerStock>();
    }
<<<<<<< HEAD
}
>>>>>>> feature/akshata/data-shops
=======
}
>>>>>>> origin/feature/hoang/pos-tier-email
