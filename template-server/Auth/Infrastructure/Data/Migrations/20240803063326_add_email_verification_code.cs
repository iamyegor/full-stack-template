using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_email_verification_code : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_refresh_tokens_users_user_id",
                table: "user_refresh_tokens"
            );

            migrationBuilder.AddColumn<int>(
                name: "email_verification_code",
                table: "users",
                type: "integer",
                nullable: true
            );

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "email_verification_code_expiry_time",
                table: "users",
                type: "timestamp with time zone",
                nullable: true
            );

            migrationBuilder.AddForeignKey(
                name: "FK_user_refresh_tokens_users_user_id",
                table: "user_refresh_tokens",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_refresh_tokens_users_user_id",
                table: "user_refresh_tokens"
            );

            migrationBuilder.DropColumn(name: "email_verification_code", table: "users");

            migrationBuilder.DropColumn(
                name: "email_verification_code_expiry_time",
                table: "users"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_user_refresh_tokens_users_user_id",
                table: "user_refresh_tokens",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );
        }
    }
}
