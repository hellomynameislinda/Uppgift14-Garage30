﻿using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
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

                await context.Database.EnsureDeletedAsync();
                await context.Database.MigrateAsync();

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
