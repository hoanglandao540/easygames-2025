
using EasyGames.Web.Data;
using EasyGames.Web.Services;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// 1) MVC + runtime compilation (easy to see Razor changes while running)
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

// 2) ONE SQLite file path for runtime (no relative path confusion)
var contentRoot = builder.Environment.ContentRootPath;               
var dbPath = Path.Combine(contentRoot, "data", "easygames.db");      // <project>\data\easygames.db
Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);           

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

// 3) DI for our services
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddHttpContextAccessor();             
builder.Services.AddSession();                          
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<EasyGames.Web.Services.ITierService, EasyGames.Web.Services.TierService>();   
builder.Services.AddScoped<EasyGames.Web.Services.IEmailService, EasyGames.Web.Services.EmailService>(); 
builder.Services.AddScoped<EasyGames.Web.Services.IPosCartService, EasyGames.Web.Services.PosCartService>(); 




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

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseStatusCodePages();
}


// 6) Middleware
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

// 7) Routes: Areas FIRST, then default
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


