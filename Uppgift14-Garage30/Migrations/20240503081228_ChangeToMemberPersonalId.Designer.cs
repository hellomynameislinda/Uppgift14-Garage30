﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Uppgift14_Garage30.Data;

#nullable disable

namespace Uppgift14_Garage30.Migrations
{
    [DbContext(typeof(Uppgift14_Garage30Context))]
    [Migration("20240503081228_ChangeToMemberPersonalId")]
    partial class ChangeToMemberPersonalId
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Uppgift14_Garage30.Models.CurrentParking", b =>
                {
                    b.Property<string>("RegistrationNumber")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("ParkingStarted")
                        .HasColumnType("datetime2");

                    b.HasKey("RegistrationNumber", "ParkingStarted");

                    b.HasIndex("RegistrationNumber")
                        .IsUnique();

                    b.ToTable("CurrentParking");
                });

            modelBuilder.Entity("Uppgift14_Garage30.Models.Member", b =>
                {
                    b.Property<string>("PersonalId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PersonalId");

                    b.ToTable("Member");
                });

            modelBuilder.Entity("Uppgift14_Garage30.Models.Vehicle", b =>
                {
                    b.Property<string>("RegistrationNumber")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Make")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MemberPersonalId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("VehicleTypeId")
                        .HasColumnType("int");

                    b.HasKey("RegistrationNumber");

                    b.HasIndex("MemberPersonalId");

                    b.HasIndex("VehicleTypeId");

                    b.ToTable("Vehicle");
                });

            modelBuilder.Entity("Uppgift14_Garage30.Models.VehicleType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("VehicleType");
                });

            modelBuilder.Entity("Uppgift14_Garage30.Models.CurrentParking", b =>
                {
                    b.HasOne("Uppgift14_Garage30.Models.Vehicle", "Vehicle")
                        .WithOne("CurrentParking")
                        .HasForeignKey("Uppgift14_Garage30.Models.CurrentParking", "RegistrationNumber")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Vehicle");
                });

            modelBuilder.Entity("Uppgift14_Garage30.Models.Vehicle", b =>
                {
                    b.HasOne("Uppgift14_Garage30.Models.Member", "Member")
                        .WithMany("Vehicles")
                        .HasForeignKey("MemberPersonalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Uppgift14_Garage30.Models.VehicleType", "VehicleType")
                        .WithMany("Vehicles")
                        .HasForeignKey("VehicleTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Member");

                    b.Navigation("VehicleType");
                });

            modelBuilder.Entity("Uppgift14_Garage30.Models.Member", b =>
                {
                    b.Navigation("Vehicles");
                });

            modelBuilder.Entity("Uppgift14_Garage30.Models.Vehicle", b =>
                {
                    b.Navigation("CurrentParking");
                });

            modelBuilder.Entity("Uppgift14_Garage30.Models.VehicleType", b =>
                {
                    b.Navigation("Vehicles");
                });
#pragma warning restore 612, 618
        }
    }
}
