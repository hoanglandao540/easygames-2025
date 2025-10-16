using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EasyGames.Web.Data
{
    // Explicit design-time factory so EF Tools stop guessing/duplicating
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var contentRoot = Directory.GetCurrentDirectory();
            // point to the SAME db path you use at runtime
            var dbPath = Path.Combine(contentRoot, "data", "easygames.db");

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite($"Data Source={dbPath}")
                .Options;

            return new AppDbContext(options);
        }
    }
}


