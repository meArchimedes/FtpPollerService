using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FtpPoller.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSubscriptionTableEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PayerId",
                table: "CopiedFiles",
                newName: "ServerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ServerId",
                table: "CopiedFiles",
                newName: "PayerId");
        }
    }
}
