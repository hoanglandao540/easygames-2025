// ===== usings =====
using EasyGames.Web.Data;
using EasyGames.Web.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// 1) MVC + runtime compilation (see Razor changes without restarting)
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

// 2) ONE SQLite file path for runtime (no relative path confusion)
var contentRoot = builder.Environment.ContentRootPath;
var dbPath = Path.Combine(contentRoot, "data", "easygames.db");   // <project>\data\easygames.db
Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

// 3) DI for our services
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ITierService, TierService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IPosCartService, PosCartService>();
builder.Services.AddHttpContextAccessor();

// 4) Session/Cache (cart + toasts)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(o =>
{
    o.IdleTimeout = TimeSpan.FromMinutes(30);
    o.Cookie.HttpOnly = true;
    o.Cookie.IsEssential = true;
});

// 5) Cookie Auth (MUST be before Build)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opt =>
    {
        opt.LoginPath = "/Account/Login";
        opt.AccessDeniedPath = "/Account/Denied";
        opt.SlidingExpiration = true;
    });
builder.Services.AddAuthorization();

// ===== build AFTER all Add... above =====
var app = builder.Build();

// 6) Prod safety
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
    app.UseStatusCodePages();
}

// 7) Middleware order
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

// 8) Auto-migrate BEFORE first requests, then seed once
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();  // create/upgrade tables
    DbSeeder.Seed(db);      // insert sample rows once
}

// 9) Routes: Areas FIRST, then default
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();