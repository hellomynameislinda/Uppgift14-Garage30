using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uppgift14_Garage30.Migrations
{
    /// <inheritdoc />
    public partial class VehicleChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MemberId",
                table: "Vehicle");

            migrationBuilder.AddColumn<string>(
                name: "PersonalId",
                table: "Vehicle",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PersonalId",
                table: "Vehicle");

            migrationBuilder.AddColumn<int>(
                name: "MemberId",
                table: "Vehicle",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
