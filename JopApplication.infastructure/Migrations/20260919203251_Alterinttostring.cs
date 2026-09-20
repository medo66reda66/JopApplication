using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JopApplication.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Alterinttostring : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CandidatJopApplications_AspNetUsers_AppuserId1",
                table: "CandidatJopApplications");

            migrationBuilder.DropIndex(
                name: "IX_CandidatJopApplications_AppuserId1",
                table: "CandidatJopApplications");

            migrationBuilder.DropColumn(
                name: "AppuserId1",
                table: "CandidatJopApplications");

            migrationBuilder.AlterColumn<string>(
                name: "AppuserId",
                table: "CandidatJopApplications",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_CandidatJopApplications_AppuserId",
                table: "CandidatJopApplications",
                column: "AppuserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CandidatJopApplications_AspNetUsers_AppuserId",
                table: "CandidatJopApplications",
                column: "AppuserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CandidatJopApplications_AspNetUsers_AppuserId",
                table: "CandidatJopApplications");

            migrationBuilder.DropIndex(
                name: "IX_CandidatJopApplications_AppuserId",
                table: "CandidatJopApplications");

            migrationBuilder.AlterColumn<int>(
                name: "AppuserId",
                table: "CandidatJopApplications",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "AppuserId1",
                table: "CandidatJopApplications",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CandidatJopApplications_AppuserId1",
                table: "CandidatJopApplications",
                column: "AppuserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_CandidatJopApplications_AspNetUsers_AppuserId1",
                table: "CandidatJopApplications",
                column: "AppuserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
