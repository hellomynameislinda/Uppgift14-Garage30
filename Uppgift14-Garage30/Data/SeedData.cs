using Bogus;
using Bogus.Extensions.Sweden;
using Microsoft.EntityFrameworkCore;
using Uppgift14_Garage30.Models;

namespace Uppgift14_Garage30.Data
{
    public class SeedData
    {

        private static Faker faker;

        public static async Task InitSeedDataAsync(Uppgift14_Garage30Context context)
        {
            faker = new Faker("sv");

            if (!await context.VehicleType.AnyAsync()) // If VehicleTable is empty, add new vehicle types
            {
                await context.AddRangeAsync(SeedVehicleTypes());
            }

            if (!await context.Member.AnyAsync())
            {
                await context.AddRangeAsync(SeedMembers(20));
            }

            if (!await context.Vehicle.AnyAsync())
            {
                await context.AddRangeAsync(SeedVehicles(20));
            }

            await context.SaveChangesAsync();

        }

        private static IEnumerable<VehicleType> SeedVehicleTypes()
        {
            // Adding static values for the seeding of VehicleTypes

            var vehicleTypes = new List<VehicleType>();

            vehicleTypes.Add(new VehicleType { Name = "Car" });
            vehicleTypes.Add(new VehicleType { Name = "Bus" });
            vehicleTypes.Add(new VehicleType { Name = "Truck" });
            vehicleTypes.Add(new VehicleType { Name = "Motorcycle" });
            vehicleTypes.Add(new VehicleType { Name = "Airplane" });
            vehicleTypes.Add(new VehicleType { Name = "Boat" });

            return vehicleTypes;
        }

        private static IEnumerable<Member> SeedMembers(int seedAmount)
        {
            var members = new List<Member>();

            Random rnd = new Random();

            for (int i = 0; i < seedAmount; i++)
            {
                // Turns out randomizing unique "Personnummer" is a bit of a mess.
                // Usin faker returns only a static property, and randomizing dates is messy.
                // First we create a random date, using the current timestamp, removing between
                // 15-85 years (to den up within a reasonable drinving range, but with someunder
                // 18 and some above 65, for testing purposes. We then add up to 12 months and
                // 31 days to get a range of dates.
                // Even so, we can't be sure the Personnummer is unique, so the last four digits
                // are made up of the counter, with leading zeros.

                DateTime rndDOB = DateTime.Now.AddYears(rnd.Next(-85, -15)).AddMonths(rnd.Next(12)).AddDays(rnd.Next(31));
                string rndFour= $"{i}".PadLeft(4, '0');
                var rndPersonalId = $"{rndDOB.ToString("yMMdd")}{rndFour}";
                members.Add(new Member
                {
                    PersonalId = rndPersonalId,
                    FirstName = faker.Name.FirstName(),
                    LastName = faker.Name.LastName()

                });
            }

            return members;
        }

        private static IEnumerable<Vehicle> SeedVehicles(int seedAmount)
        {
            var vehicles = new List<Vehicle>();



            return vehicles;
        }

        // TODO: Seed current parking
    }
}
