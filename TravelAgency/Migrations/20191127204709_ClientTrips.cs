using Microsoft.EntityFrameworkCore.Migrations;

namespace TravelAgency.Migrations
{
    public partial class ClientTrips : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "Trips",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trips_ClientId",
                table: "Trips",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Clients_ClientId",
                table: "Trips",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Clients_ClientId",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Trips_ClientId",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Trips");

            migrationBuilder.AddColumn<int>(
                name: "TripId",
                table: "Clients",
                type: "int",
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
    }
}
