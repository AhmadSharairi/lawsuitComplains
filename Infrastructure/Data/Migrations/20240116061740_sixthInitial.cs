using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    public partial class sixthInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersAuthentication_Users_UserId",
                table: "UsersAuthentication");

            migrationBuilder.DropIndex(
                name: "IX_UsersAuthentication_UserId",
                table: "UsersAuthentication");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UsersAuthentication");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "UsersAuthentication",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UsersAuthentication_UserId",
                table: "UsersAuthentication",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersAuthentication_Users_UserId",
                table: "UsersAuthentication",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
