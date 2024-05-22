using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightBooking.Migrations
{
    public partial class OtherTbls : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_airport",
                columns: table => new
                {
                    AirportId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_airport", x => x.AirportId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_flight",
                columns: table => new
                {
                    FlightId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlightNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartureTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ArrivalTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DepartureAirportId = table.Column<int>(type: "int", nullable: false),
                    ArrivalAirportId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_flight", x => x.FlightId);
                    table.ForeignKey(
                        name: "FK_tbl_flight_tbl_airport_ArrivalAirportId",
                        column: x => x.ArrivalAirportId,
                        principalTable: "tbl_airport",
                        principalColumn: "AirportId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_flight_tbl_airport_DepartureAirportId",
                        column: x => x.DepartureAirportId,
                        principalTable: "tbl_airport",
                        principalColumn: "AirportId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_booking",
                columns: table => new
                {
                    BookingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlightId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_booking", x => x.BookingId);
                    table.ForeignKey(
                        name: "FK_tbl_booking_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_booking_tbl_flight_FlightId",
                        column: x => x.FlightId,
                        principalTable: "tbl_flight",
                        principalColumn: "FlightId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_booking_FlightId",
                table: "tbl_booking",
                column: "FlightId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_booking_UserId",
                table: "tbl_booking",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_flight_ArrivalAirportId",
                table: "tbl_flight",
                column: "ArrivalAirportId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_flight_DepartureAirportId",
                table: "tbl_flight",
                column: "DepartureAirportId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_booking");

            migrationBuilder.DropTable(
                name: "tbl_flight");

            migrationBuilder.DropTable(
                name: "tbl_airport");
        }
    }
}
