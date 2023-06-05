using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FtpPoller.Data.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CopiedFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PayerId = table.Column<Guid>(type: "uuid", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: false),
                    InsertedAt = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CopiedFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SrcServer = table.Column<string>(type: "text", nullable: false),
                    SrcPort = table.Column<int>(type: "integer", nullable: false),
                    SrcUserName = table.Column<string>(type: "text", nullable: false),
                    SrcPassword = table.Column<string>(type: "text", nullable: false),
                    FolderDir = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payers", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CopiedFiles");

            migrationBuilder.DropTable(
                name: "Payers");
        }
    }
}
