using LibraryManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Password rules
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;

    // User settings
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<LibraryContext>()
.AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";

    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.SlidingExpiration = true;
});
var app = builder.Build();

// Configure middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    await IdentitySeeder.SeedRolesAndAdminAsync(services);
}


app.Run();