using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FtpPoller.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRecipientServerEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payers");

            migrationBuilder.CreateTable(
                name: "PayerServers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RecipientId = table.Column<Guid>(type: "uuid", nullable: false),
                    Host = table.Column<string>(type: "text", nullable: false),
                    Port = table.Column<int>(type: "integer", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    FolderDir = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayerServers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecipientServers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Host = table.Column<string>(type: "text", nullable: false),
                    Port = table.Column<int>(type: "integer", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    FolderDir = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipientServers", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PayerServers");

            migrationBuilder.DropTable(
                name: "RecipientServers");

            migrationBuilder.CreateTable(
                name: "Payers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FolderDir = table.Column<string>(type: "text", nullable: false),
                    SrcPassword = table.Column<string>(type: "text", nullable: false),
                    SrcPort = table.Column<int>(type: "integer", nullable: false),
                    SrcServer = table.Column<string>(type: "text", nullable: false),
                    SrcUserName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payers", x => x.Id);
                });
        }
    }
}
