using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Delivery.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Users : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Users",
                newName: "username");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Users",
                newName: "password");

            migrationBuilder.RenameColumn(
                name: "RefreshToken",
                table: "Users",
                newName: "refresh_token");

            migrationBuilder.RenameColumn(
                name: "AccessToken",
                table: "Users",
                newName: "access_token");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Users",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "Distance",
                table: "Orders",
                newName: "distance");

            migrationBuilder.RenameColumn(
                name: "Cost",
                table: "Orders",
                newName: "cost");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Orders",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Orders",
                newName: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "username",
                table: "Users",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "password",
                table: "Users",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "refresh_token",
                table: "Users",
                newName: "RefreshToken");

            migrationBuilder.RenameColumn(
                name: "access_token",
                table: "Users",
                newName: "AccessToken");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "Users",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "distance",
                table: "Orders",
                newName: "Distance");

            migrationBuilder.RenameColumn(
                name: "cost",
                table: "Orders",
                newName: "Cost");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Orders",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "Orders",
                newName: "UserId");
        }
    }
}
