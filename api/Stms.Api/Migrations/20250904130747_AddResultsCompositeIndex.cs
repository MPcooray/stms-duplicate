using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stms.Api.Migrations
{
    public partial class AddResultsCompositeIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // DO NOT drop IX_Results_EventId here — it's required by the FK.
            migrationBuilder.CreateIndex(
                name: "IX_Results_EventId_TimingMs",
                table: "Results",
                columns: new[] { "EventId", "TimingMs" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Only remove the composite index on downgrade.
            migrationBuilder.DropIndex(
                name: "IX_Results_EventId_TimingMs",
                table: "Results");
        }
    }
}
