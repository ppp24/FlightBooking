using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightBooking.Migrations
{
    public partial class updatebooking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OutboundArriveAirport",
                table: "tbl_booking",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "OutboundArriveTime",
                table: "tbl_booking",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "OutboundDepartAirport",
                table: "tbl_booking",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "OutboundDepartTime",
                table: "tbl_booking",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ReturnArriveAirport",
                table: "tbl_booking",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReturnArriveTime",
                table: "tbl_booking",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReturnDepartAirport",
                table: "tbl_booking",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReturnDepartTime",
                table: "tbl_booking",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OutboundArriveAirport",
                table: "tbl_booking");

            migrationBuilder.DropColumn(
                name: "OutboundArriveTime",
                table: "tbl_booking");

            migrationBuilder.DropColumn(
                name: "OutboundDepartAirport",
                table: "tbl_booking");

            migrationBuilder.DropColumn(
                name: "OutboundDepartTime",
                table: "tbl_booking");

            migrationBuilder.DropColumn(
                name: "ReturnArriveAirport",
                table: "tbl_booking");

            migrationBuilder.DropColumn(
                name: "ReturnArriveTime",
                table: "tbl_booking");

            migrationBuilder.DropColumn(
                name: "ReturnDepartAirport",
                table: "tbl_booking");

            migrationBuilder.DropColumn(
                name: "ReturnDepartTime",
                table: "tbl_booking");
        }
    }
}
