using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightBooking.Migrations
{
    public partial class updaterbookigntbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OutboundPrice",
                table: "tbl_booking",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OutboundPriceType",
                table: "tbl_booking",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ReturnPrice",
                table: "tbl_booking",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ReturnPriceType",
                table: "tbl_booking",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OutboundPrice",
                table: "tbl_booking");

            migrationBuilder.DropColumn(
                name: "OutboundPriceType",
                table: "tbl_booking");

            migrationBuilder.DropColumn(
                name: "ReturnPrice",
                table: "tbl_booking");

            migrationBuilder.DropColumn(
                name: "ReturnPriceType",
                table: "tbl_booking");
        }
    }
}
