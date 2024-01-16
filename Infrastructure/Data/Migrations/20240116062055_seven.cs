using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    public partial class seven : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResetPasswordExpiry",
                table: "UsersAuthentication");

            migrationBuilder.DropColumn(
                name: "ResetPasswordToken",
                table: "UsersAuthentication");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ResetPasswordExpiry",
                table: "UsersAuthentication",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ResetPasswordToken",
                table: "UsersAuthentication",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
