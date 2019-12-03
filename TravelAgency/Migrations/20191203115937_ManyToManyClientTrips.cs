using Microsoft.EntityFrameworkCore.Migrations;

namespace TravelAgency.Migrations
{
    public partial class ManyToManyClientTrips : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "ClientTrip",
                columns: table => new
                {
                    ClientId = table.Column<int>(nullable: false),
                    TripId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientTrip", x => new { x.ClientId, x.TripId });
                    table.ForeignKey(
                        name: "FK_ClientTrip_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientTrip_Trips_TripId",
                        column: x => x.TripId,
                        principalTable: "Trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientTrip_TripId",
                table: "ClientTrip",
                column: "TripId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientTrip");

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "Trips",
                type: "int",
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
    }
}
