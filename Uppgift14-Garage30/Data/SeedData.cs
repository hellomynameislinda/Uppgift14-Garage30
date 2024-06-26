﻿using Bogus;
using Bogus.Extensions.Sweden;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Uppgift14_Garage30.Models;

namespace Uppgift14_Garage30.Data
{
    // SQL Script to test generated users in SQL Server:
    // *************************************************
    //SELECT m.PersonalId, m.LastName, m.FirstName, v.RegistrationNumber,
    //    vt.Name, v.Make, v.Model, v.Color, cp.ParkingStarted FROM Vehicle v

    //LEFT OUTER JOIN CurrentParking cp ON v.RegistrationNumber = cp.RegistrationNumber

    //INNER JOIN VehicleType vt ON v.VehicleTypeId = vt.Id

    //INNER JOIN Member m ON v.MemberPersonalId = m.PersonalId

    //ORDER BY m.LastName ASC, m.FirstName ASC


    public class SeedData
    {

        private static Faker faker;

        public static async Task InitSeedDataAsync(Uppgift14_Garage30Context context)
        {
            faker = new Faker("sv");

            var vehicleTypes = SeedVehicleTypes();
            await context.AddRangeAsync(vehicleTypes);

            var members = SeedMembers(60);
            await context.AddRangeAsync(members);

            var vehicles = SeedVehicles(60, vehicleTypes, members);
            await context.AddRangeAsync(vehicles);

            var currentParking = SeedCurrentParking(vehicles);
            await context.AddRangeAsync(currentParking);

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
                string rndFour = $"{i}".PadLeft(4, '0');
                var rndPersonalId = $"{rndDOB.ToString("yyyyMMdd")}{rndFour}";
                members.Add(new Member
                {
                    PersonalId = rndPersonalId,
                    FirstName = faker.Name.FirstName(),
                    LastName = faker.Name.LastName()

                });
            }

            return members;
        }

        private static IEnumerable<Vehicle> SeedVehicles(int seedAmount, IEnumerable<VehicleType> vehicleTypes, IEnumerable<Member> members)
        {
            var vehicles = new List<Vehicle>();

            List<VehicleType> tmpVehicleTypes = vehicleTypes.ToList();
            List<Member> tmpMembers = members.ToList();

            char[] validLetters = "ABCDEFGHJKLMNOPRSTUWXYZ".ToCharArray();


            Random rnd = new Random();

            for (int i = 0; i < seedAmount; i++)
            {
                string regNumber = $"{validLetters[rnd.Next(0, validLetters.Length)]}" +
                    $"{validLetters[rnd.Next(0, validLetters.Length)]}" +
                    $"{validLetters[rnd.Next(0, validLetters.Length)]}" +
                    $"{rnd.Next(0, 9)}" +
                    $"{rnd.Next(0, 9)}" +
                    $"{rnd.Next(0, 9)}";
                vehicles.Add(new Vehicle
                {
                    RegistrationNumber = regNumber,
                    Make = faker.Vehicle.Manufacturer(),
                    Model = faker.Vehicle.Model(),
                    Color = faker.Commerce.Color(),
                    VehicleType = tmpVehicleTypes[rnd.Next(0, tmpVehicleTypes.Count)],
                    Member = tmpMembers[rnd.Next(0, tmpMembers.Count)]
                });
            }
            return vehicles;
        }

        private static IEnumerable<CurrentParking> SeedCurrentParking(IEnumerable<Vehicle> vehicles)
        {
            var currentParking = new List<CurrentParking>();

            List<Vehicle> tmpVehicles = vehicles.ToList();

            Random rnd = new Random();

            for (int i = 0; i < tmpVehicles.Count; i+=3)
            {
                currentParking.Add(new CurrentParking
                {
                    RegistrationNumber = tmpVehicles[i].RegistrationNumber,
                    ParkingStarted = DateTime.Now.AddMinutes(rnd.Next(-2880, 0)) // Randomize within the last 48 hours
                });
            }
            return currentParking;
        }
    }
}
