<<<<<<< HEAD
<<<<<<< HEAD
﻿using EasyGames.Web.Data;
=======
﻿// ===== usings =====
using EasyGames.Web.Data;
>>>>>>> feature/akshata/data-shops
=======
﻿using EasyGames.Web.Data;
>>>>>>> origin/feature/hoang/pos-tier-email
using EasyGames.Web.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

<<<<<<< HEAD
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
=======
// 1) MVC + runtime compilation
>>>>>>> origin/feature/hoang/pos-tier-email
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

// 2) ONE SQLite DbContext registration (no duplicates!)
var contentRoot = builder.Environment.ContentRootPath;
<<<<<<< HEAD
var dbPath = Path.Combine(contentRoot, "data", "easygames.db");   // <project>\data\easygames.db
Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);
>>>>>>> feature/akshata/data-shops
=======
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
>>>>>>> origin/feature/hoang/pos-tier-email

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

<<<<<<< HEAD
<<<<<<< HEAD
// 3) DI for services
=======
// 3) DI for our services
>>>>>>> feature/akshata/data-shops
=======
// 3) DI for services
>>>>>>> origin/feature/hoang/pos-tier-email
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ITierService, TierService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IPosCartService, PosCartService>();
builder.Services.AddHttpContextAccessor();

<<<<<<< HEAD
<<<<<<< HEAD
// 4) Session/Cache
=======
// 4) Session/Cache (cart + toasts)
>>>>>>> feature/akshata/data-shops
=======
// 4) Session/Cache
>>>>>>> origin/feature/hoang/pos-tier-email
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(o =>
{
    o.IdleTimeout = TimeSpan.FromMinutes(30);
    o.Cookie.HttpOnly = true;
    o.Cookie.IsEssential = true;
});

<<<<<<< HEAD
<<<<<<< HEAD
// 5) Cookie Auth
=======
// 5) Cookie Auth (MUST be before Build)
>>>>>>> feature/akshata/data-shops
=======
// 5) Cookie Auth
>>>>>>> origin/feature/hoang/pos-tier-email
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opt =>
    {
        opt.LoginPath = "/Account/LoginCustomer";
        opt.AccessDeniedPath = "/Account/Denied";
        opt.SlidingExpiration = true;
    });
builder.Services.AddAuthorization();

<<<<<<< HEAD
<<<<<<< HEAD
// ===== BUILD =====
=======
// ===== build AFTER all Add... above =====
>>>>>>> feature/akshata/data-shops
=======
// ===== BUILD =====
>>>>>>> origin/feature/hoang/pos-tier-email
var app = builder.Build();

// 6) Prod safety
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
<<<<<<< HEAD
<<<<<<< HEAD
=======
    app.UseStatusCodePages();
>>>>>>> feature/akshata/data-shops
=======
>>>>>>> origin/feature/hoang/pos-tier-email
}

// 7) Middleware order
app.UseHttpsRedirection();
app.UseStaticFiles();
<<<<<<< HEAD
<<<<<<< HEAD
app.UseRouting();
=======

app.UseRouting();

>>>>>>> feature/akshata/data-shops
=======
app.UseRouting();
>>>>>>> origin/feature/hoang/pos-tier-email
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

<<<<<<< HEAD
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
=======
// 8) Auto-migrate & seed
>>>>>>> origin/feature/hoang/pos-tier-email
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

<<<<<<< HEAD
// 9) Routes: Areas FIRST, then default
>>>>>>> feature/akshata/data-shops
=======
// 9) Routes
>>>>>>> origin/feature/hoang/pos-tier-email
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

<<<<<<< HEAD
<<<<<<< HEAD
app.Run();
=======
app.Run();
>>>>>>> feature/akshata/data-shops
=======
app.Run();
>>>>>>> origin/feature/hoang/pos-tier-email
