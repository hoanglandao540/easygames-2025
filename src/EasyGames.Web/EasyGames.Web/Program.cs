<<<<<<< HEAD
﻿using EasyGames.Web.Data;
=======
﻿// ===== usings =====
using EasyGames.Web.Data;
>>>>>>> feature/akshata/data-shops
using EasyGames.Web.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

<<<<<<< HEAD
// 1) MVC + runtime compilation
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

// 2) ONE SQLite DbContext registration (no duplicates!)
var contentRoot = builder.Environment.ContentRootPath;
var dbPath = Path.Combine(contentRoot, "data", "easygames.db");

// FIX: Add error handling for directory creation
try
{
    var dbDir = Path.GetDirectoryName(dbPath);
    if (!string.IsNullOrEmpty(dbDir) && !Directory.Exists(dbDir))
    {
        Directory.CreateDirectory(dbDir);
    }
}
catch (Exception ex)
{
    throw new InvalidOperationException($"Failed to create database directory at {dbPath}", ex);
}
=======
// 1) MVC + runtime compilation (see Razor changes without restarting)
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

// 2) ONE SQLite file path for runtime (no relative path confusion)
var contentRoot = builder.Environment.ContentRootPath;
var dbPath = Path.Combine(contentRoot, "data", "easygames.db");   // <project>\data\easygames.db
Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);
>>>>>>> feature/akshata/data-shops

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

<<<<<<< HEAD
// 3) DI for services
=======
// 3) DI for our services
>>>>>>> feature/akshata/data-shops
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ITierService, TierService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IPosCartService, PosCartService>();
builder.Services.AddHttpContextAccessor();

<<<<<<< HEAD
// 4) Session/Cache
=======
// 4) Session/Cache (cart + toasts)
>>>>>>> feature/akshata/data-shops
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(o =>
{
    o.IdleTimeout = TimeSpan.FromMinutes(30);
    o.Cookie.HttpOnly = true;
    o.Cookie.IsEssential = true;
});

<<<<<<< HEAD
// 5) Cookie Auth
=======
// 5) Cookie Auth (MUST be before Build)
>>>>>>> feature/akshata/data-shops
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opt =>
    {
        opt.LoginPath = "/Account/LoginCustomer";
        opt.AccessDeniedPath = "/Account/Denied";
        opt.SlidingExpiration = true;
    });
builder.Services.AddAuthorization();

<<<<<<< HEAD
// ===== BUILD =====
=======
// ===== build AFTER all Add... above =====
>>>>>>> feature/akshata/data-shops
var app = builder.Build();

// 6) Prod safety
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
<<<<<<< HEAD
=======
    app.UseStatusCodePages();
>>>>>>> feature/akshata/data-shops
}

// 7) Middleware order
app.UseHttpsRedirection();
app.UseStaticFiles();
<<<<<<< HEAD
app.UseRouting();
=======

app.UseRouting();

>>>>>>> feature/akshata/data-shops
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

<<<<<<< HEAD
// 8) Auto-migrate & seed
using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.Migrate();
        DbSeeder.Seed(db);
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred during database migration and seeding");
        throw;
    }
}

// 9) Routes
=======
// 8) Auto-migrate BEFORE first requests, then seed once
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();  // create/upgrade tables
    DbSeeder.Seed(db);      // insert sample rows once
}

// 9) Routes: Areas FIRST, then default
>>>>>>> feature/akshata/data-shops
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

<<<<<<< HEAD
app.Run();
=======
app.Run();
>>>>>>> feature/akshata/data-shops
