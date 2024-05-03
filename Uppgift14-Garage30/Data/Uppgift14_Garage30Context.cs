using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Uppgift14_Garage30.Models;

namespace Uppgift14_Garage30.Data
{
    public class Uppgift14_Garage30Context : DbContext
    {
        public DbSet<Uppgift14_Garage30.Models.Member> Member { get; set; } = default!;
        public DbSet<Uppgift14_Garage30.Models.VehicleType> VehicleType { get; set; } = default!;
        public DbSet<Uppgift14_Garage30.Models.Vehicle> Vehicle { get; set; } = default!;
        public DbSet<Uppgift14_Garage30.Models.CurrentParking> CurrentParking { get; set; } = default!;

        public Uppgift14_Garage30Context (DbContextOptions<Uppgift14_Garage30Context> options)
            : base(options)
        {
        }

        // TODO: Fix this relationship
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CurrentParking>()
                .HasKey(cp => new { cp.RegistrationNumber, cp.ParkingStarted });

            modelBuilder.Entity<Vehicle>()
                .HasOne(e => e.CurrentParking)
                .WithOne(e => e.Vehicle)
                .HasForeignKey<CurrentParking>(e => e.RegistrationNumber);
        }

    }
}
