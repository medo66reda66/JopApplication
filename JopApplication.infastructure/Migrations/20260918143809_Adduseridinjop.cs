using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JopApplication.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Adduseridinjop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppuserId",
                table: "Jops",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Jops_AppuserId",
                table: "Jops",
                column: "AppuserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Jops_AspNetUsers_AppuserId",
                table: "Jops",
                column: "AppuserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jops_AspNetUsers_AppuserId",
                table: "Jops");

            migrationBuilder.DropIndex(
                name: "IX_Jops_AppuserId",
                table: "Jops");

            migrationBuilder.DropColumn(
                name: "AppuserId",
                table: "Jops");
        }
    }
}
