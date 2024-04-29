using Hv.Ppb302.DigitalThesis.WebClient.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace Hv.Ppb302.DigitalThesis.WebClient;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<DigitalThesisDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("SQLServerConnection")),
            ServiceLifetime.Scoped);

        builder.Services.AddScoped<GeoTagRepository>();
        builder.Services.AddScoped<GroupTagRepository>();
        builder.Services.AddScoped<MolarMosaicRepository>();
        builder.Services.AddScoped<MolecularMosaicRepository>();
        builder.Services.AddScoped<TestDataUtils>();

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        // Lägg till stöd för loginfunktionen. Skicka med rätt sida för LoginPath
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options => { options.LoginPath = "/Admin/Index/"; });
        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
        }
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseSession();

        app.MapControllers();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}