using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MailSystem.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddSpamRules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SpamRules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerId = table.Column<int>(type: "int", nullable: false),
                    SenderEmail = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsWhitelist = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpamRules", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpamRules_OwnerId_SenderEmail",
                table: "SpamRules",
                columns: new[] { "OwnerId", "SenderEmail" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpamRules");
        }
    }
}
