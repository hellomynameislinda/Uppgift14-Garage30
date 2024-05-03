using Bogus;
using Microsoft.EntityFrameworkCore;
using Uppgift14_Garage30.Models;

namespace Uppgift14_Garage30.Data
{
    public class SeedData
    {

        private static Faker faker;

        public static async Task InitSeedDataAsync(Uppgift14_Garage30Context context)
        {
            if (!await context.VehicleType.AnyAsync()) // If VehicleTable is empty, add new vehicle types
            {
                await context.AddRangeAsync(SeedVehicleTypes());
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

        private static IEnumerable<Member> SeedMembers()
        {
            var members = new List<Member>();
            return members;
        }

        private static IEnumerable<Vehicle> SeedVehicles()
        {
            var vehicles = new List<Vehicle>();
            return vehicles;
        }

        // TODO: Seed current parking
    }
}
