using Microsoft.CodeAnalysis.CSharp.Syntax;
using Uppgift14_Garage30.Data;

namespace Uppgift14_Garage30.Extensions
{
    public static class SeedingExtensions
    {

        public static async Task SeedDataAsync(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var context = serviceProvider.GetRequiredService<Uppgift14_Garage30Context>();

                try
                {
                    await SeedData.InitSeedDataAsync(context);
                }
                catch (Exception ex) 
                { 
                   Console.WriteLine(ex.Message);
                    throw;
                }

            }
        }
    }
}
