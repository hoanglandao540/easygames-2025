// student-style: make PMC/CLI use the SAME SQLite file as runtime
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.IO;

namespace EasyGames.Web.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            // Resolve <web project>\data\easygames.db
            var projectDir = Directory.GetCurrentDirectory();
            var dbPath = Path.Combine(projectDir, "data", "easygames.db");
            Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite($"Data Source={dbPath}")
                .Options;

            return new AppDbContext(options);
        }
    }
}


