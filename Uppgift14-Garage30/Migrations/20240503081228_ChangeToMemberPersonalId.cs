using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uppgift14_Garage30.Migrations
{
    /// <inheritdoc />
    public partial class ChangeToMemberPersonalId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicle_Member_MemberPersonalId",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "PersonalId",
                table: "Vehicle");

            migrationBuilder.AlterColumn<string>(
                name: "MemberPersonalId",
                table: "Vehicle",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicle_Member_MemberPersonalId",
                table: "Vehicle",
                column: "MemberPersonalId",
                principalTable: "Member",
                principalColumn: "PersonalId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicle_Member_MemberPersonalId",
                table: "Vehicle");

            migrationBuilder.AlterColumn<string>(
                name: "MemberPersonalId",
                table: "Vehicle",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "PersonalId",
                table: "Vehicle",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicle_Member_MemberPersonalId",
                table: "Vehicle",
                column: "MemberPersonalId",
                principalTable: "Member",
                principalColumn: "PersonalId");
        }
    }
}
