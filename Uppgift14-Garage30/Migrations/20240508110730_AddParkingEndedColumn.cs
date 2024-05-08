using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uppgift14_Garage30.Migrations
{
    /// <inheritdoc />
    public partial class AddParkingEndedColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ParkingEnded",
                table: "CurrentParking",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParkingEnded",
                table: "CurrentParking");
        }
    }
}
