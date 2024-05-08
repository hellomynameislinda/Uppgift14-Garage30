using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Uppgift14_Garage30.Data;
using Uppgift14_Garage30.Extensions;
namespace Uppgift14_Garage30
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<Uppgift14_Garage30Context>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("Uppgift14_Garage30Context") ?? throw new InvalidOperationException("Connection string 'Uppgift14_Garage30Context' not found.")));

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            }); // Added to use session for login

            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); // Needed to access the session above.

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            else
            {
                await app.SeedDataAsync();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession(); // Added to use session for login

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
