using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uppgift14_Garage30.Migrations
{
    /// <inheritdoc />
    public partial class AddingCurrentParking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CurrentParking",
                columns: table => new
                {
                    RegistrationNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ParkingStarted = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrentParking", x => new { x.RegistrationNumber, x.ParkingStarted });
                    table.ForeignKey(
                        name: "FK_CurrentParking_Vehicle_RegistrationNumber",
                        column: x => x.RegistrationNumber,
                        principalTable: "Vehicle",
                        principalColumn: "RegistrationNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CurrentParking_RegistrationNumber",
                table: "CurrentParking",
                column: "RegistrationNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurrentParking");
        }
    }
}
