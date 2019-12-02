using Microsoft.EntityFrameworkCore.Migrations;

namespace TravelAgency.Migrations
{
    public partial class TripClients : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TripId",
                table: "Clients",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_TripId",
                table: "Clients",
                column: "TripId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Trips_TripId",
                table: "Clients",
                column: "TripId",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Trips_TripId",
                table: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_Clients_TripId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "TripId",
                table: "Clients");
        }
    }
}
