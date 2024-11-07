using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_name_instead_of_login : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(name: "IX_users_login", table: "users");

            migrationBuilder.RenameColumn(name: "login", table: "users", newName: "name");

            migrationBuilder.AlterColumn<string>(
                name: "hashed_password",
                table: "users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text"
            );

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text"
            );

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(name: "name", table: "users", newName: "login");

            migrationBuilder.AlterColumn<string>(
                name: "hashed_password",
                table: "users",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true
            );

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "users",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true
            );

            migrationBuilder.AlterColumn<string>(
                name: "login",
                table: "users",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_users_login",
                table: "users",
                column: "login",
                unique: true
            );
        }
    }
}
