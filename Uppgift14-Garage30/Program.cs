using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Uppgift14_Garage30.Data;
namespace Uppgift14_Garage30
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<Uppgift14_Garage30Context>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("Uppgift14_Garage30Context") ?? throw new InvalidOperationException("Connection string 'Uppgift14_Garage30Context' not found.")));

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
