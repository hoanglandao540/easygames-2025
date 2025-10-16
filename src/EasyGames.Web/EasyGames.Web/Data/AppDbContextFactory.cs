<<<<<<< HEAD
<<<<<<< HEAD
﻿using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EasyGames.Web.Data
{
    // Explicit design-time factory so EF Tools stop guessing/duplicating
=======
﻿// student-style: make PMC/CLI use the SAME SQLite file as runtime
=======
﻿using System.IO;
>>>>>>> origin/feature/hoang/pos-tier-email
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EasyGames.Web.Data
{
<<<<<<< HEAD
>>>>>>> feature/akshata/data-shops
=======
    // Explicit design-time factory so EF Tools stop guessing/duplicating
>>>>>>> origin/feature/hoang/pos-tier-email
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
<<<<<<< HEAD
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
=======
            var contentRoot = Directory.GetCurrentDirectory();
            // point to the SAME db path you use at runtime
            var dbPath = Path.Combine(contentRoot, "data", "easygames.db");
>>>>>>> origin/feature/hoang/pos-tier-email

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite($"Data Source={dbPath}")
                .Options;

            return new AppDbContext(options);
        }
    }
}


