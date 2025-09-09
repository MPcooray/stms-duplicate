using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Stms.Api.Migrations
{
    /// <inheritdoc />
    public partial class Sprint2_RankingAndPoints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Points",
                table: "Results",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Rank",
                table: "Results",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ScoringRules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Place = table.Column<int>(type: "int", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoringRules", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "ScoringRules",
                columns: new[] { "Id", "Place", "Points" },
                values: new object[,]
                {
                    { 1, 1, 9 },
                    { 2, 2, 7 },
                    { 3, 3, 6 },
                    { 4, 4, 5 },
                    { 5, 5, 4 },
                    { 6, 6, 3 },
                    { 7, 7, 2 },
                    { 8, 8, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScoringRules_Place",
                table: "ScoringRules",
                column: "Place",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScoringRules");

            migrationBuilder.DropColumn(
                name: "Points",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "Rank",
                table: "Results");
        }
    }
}
