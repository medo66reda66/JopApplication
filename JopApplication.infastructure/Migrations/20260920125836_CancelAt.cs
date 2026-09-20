using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JopApplication.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CancelAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CancelAt",
                table: "CandidatJopApplications",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CancelAt",
                table: "CandidatJopApplications");
        }
    }
}
