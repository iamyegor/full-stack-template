using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    login = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    hashed_password = table.Column<string>(type: "text", nullable: false),
                    is_email_verified = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                }
            );

            migrationBuilder.CreateTable(
                name: "user_refresh_tokens",
                columns: table => new
                {
                    id = table
                        .Column<int>(type: "integer", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    device_id = table.Column<Guid>(type: "uuid", nullable: false),
                    value = table.Column<string>(type: "text", nullable: false),
                    expiry_time = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                    user_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_refresh_tokens", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_refresh_tokens_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id"
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_user_refresh_tokens_user_id",
                table: "user_refresh_tokens",
                column: "user_id"
            );

            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                table: "users",
                column: "email",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_users_login",
                table: "users",
                column: "login",
                unique: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "user_refresh_tokens");

            migrationBuilder.DropTable(name: "users");
        }
    }
}
