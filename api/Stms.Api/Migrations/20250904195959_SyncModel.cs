using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stms.Api.Migrations
{
    /// <inheritdoc />
    public partial class SyncModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "Players");

            migrationBuilder.RenameColumn(
                name: "Gender",
                table: "Players",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "Players",
                newName: "FirstName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Players",
                newName: "Gender");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Players",
                newName: "FullName");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "Players",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ExternalId",
                table: "Players",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
