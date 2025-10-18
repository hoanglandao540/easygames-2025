using EasyGames.Web.Data;
using EasyGames.Web.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// 1) MVC + runtime compilation
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

// 2) SQLite DbContext
var contentRoot = builder.Environment.ContentRootPath;
var dbPath = Path.Combine(contentRoot, "data", "easygames.db");

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

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

// 3) DI for services
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ITierService, TierService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IPosCartService, PosCartService>();
builder.Services.AddHttpContextAccessor();

// 4) Session/Cache
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(o =>
{
    o.IdleTimeout = TimeSpan.FromMinutes(30);
    o.Cookie.HttpOnly = true;
    o.Cookie.IsEssential = true;
});

// 5) Cookie Auth
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opt =>
    {
        opt.LoginPath = "/Account/LoginCustomer";
        opt.AccessDeniedPath = "/Account/Denied";
        opt.SlidingExpiration = true;
    });
builder.Services.AddAuthorization();

var app = builder.Build();

// 6) Prod safety
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// 7) Middleware order
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

// 8) FIXED: Safe database initialization
using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        // Use EnsureCreated for simple scenarios (creates DB if not exists, does nothing if exists)
        // This won't conflict with existing tables
        if (db.Database.EnsureCreated())
        {
            logger.LogInformation("Database created successfully.");
        }
        else
        {
            logger.LogInformation("Database already exists.");
        }

        // Seed data (DbSeeder.Seed should check if data exists before inserting)
        DbSeeder.Seed(db);
        logger.LogInformation("Database seeding completed.");
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred during database initialization");

        if (app.Environment.IsDevelopment())
        {
            logger.LogError("💡 TIP: Delete the database file at {DbPath} and restart", dbPath);
        }

        throw;
    }
}

// 9) Routes
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();