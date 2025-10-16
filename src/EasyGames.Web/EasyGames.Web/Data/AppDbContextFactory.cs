<<<<<<< HEAD
﻿using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EasyGames.Web.Data
{
    // Explicit design-time factory so EF Tools stop guessing/duplicating
=======
﻿// student-style: make PMC/CLI use the SAME SQLite file as runtime
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.IO;

namespace EasyGames.Web.Data
{
>>>>>>> feature/akshata/data-shops
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
<<<<<<< HEAD
            var contentRoot = Directory.GetCurrentDirectory();
            // point to the SAME db path you use at runtime
            var dbPath = Path.Combine(contentRoot, "data", "easygames.db");
=======
            // Resolve <web project>\data\easygames.db
            var projectDir = Directory.GetCurrentDirectory();
            var dbPath = Path.Combine(projectDir, "data", "easygames.db");
            Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);
>>>>>>> feature/akshata/data-shops

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite($"Data Source={dbPath}")
                .Options;

            return new AppDbContext(options);
        }
    }
}


