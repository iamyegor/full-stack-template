using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class password_reset_token : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "password_reset_token",
                table: "users",
                type: "uuid",
                nullable: true
            );

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "password_reset_token_expiry_time",
                table: "users",
                type: "timestamp with time zone",
                nullable: true
            );

            migrationBuilder.AlterColumn<Guid>(
                name: "user_id",
                table: "user_refresh_tokens",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "password_reset_token", table: "users");

            migrationBuilder.DropColumn(name: "password_reset_token_expiry_time", table: "users");

            migrationBuilder.AlterColumn<Guid>(
                name: "user_id",
                table: "user_refresh_tokens",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid"
            );
        }
    }
}
