using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FtpPoller.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovedRefFromPayerEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecipientId",
                table: "PayerServers");

            migrationBuilder.RenameColumn(
                name: "ServerId",
                table: "CopiedFiles",
                newName: "PayerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PayerId",
                table: "CopiedFiles",
                newName: "ServerId");

            migrationBuilder.AddColumn<Guid>(
                name: "RecipientId",
                table: "PayerServers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
