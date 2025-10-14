// File: src/EasyGames.Web/Program.cs
// student-style: full Program.cs with a SINGLE absolute SQLite path (same every run)

using EasyGames.Web.Data;
using EasyGames.Web.Services;
using Microsoft.EntityFrameworkCore;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// 1) MVC + runtime compilation (easy to see Razor changes while running)
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

// 2) ONE SQLite file path for runtime (no relative path confusion)
var contentRoot = builder.Environment.ContentRootPath;               // <project folder>
var dbPath = Path.Combine(contentRoot, "data", "easygames.db");      // <project>\data\easygames.db
Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);           // make sure \data exists

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

// 3) DI for our services
builder.Services.AddScoped<IInventoryService, InventoryService>();

var app = builder.Build();

// 4) Standard prod safety
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// 5) Auto-migrate BEFORE seeding (important order)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();   // create/upgrade tables
    DbSeeder.Seed(db);       // insert sample rows once
}

// 6) Middleware
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// 7) Routes: Areas FIRST, then default
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


