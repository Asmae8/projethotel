using Microsoft.EntityFrameworkCore;
using SystemeHotel;
using SystemeHotel.Models;

var builder = WebApplication.CreateBuilder(args);

// Ajouter MVC
builder.Services.AddControllersWithViews();

// Ajouter DbContext (Pomelo MySQL)
var connectionString = builder.Configuration.GetConnectionString("Nom_Con");
builder.Services.AddDbContext<HotelDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

var app = builder.Build();

// Middleware
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

// ROUTAGE MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

