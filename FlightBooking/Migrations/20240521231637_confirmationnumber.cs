using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightBooking.Migrations
{
    public partial class confirmationnumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TblPayments_tbl_booking_BookingId",
                table: "TblPayments");

            migrationBuilder.DropIndex(
                name: "IX_TblPayments_BookingId",
                table: "TblPayments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TblPayments_BookingId",
                table: "TblPayments",
                column: "BookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_TblPayments_tbl_booking_BookingId",
                table: "TblPayments",
                column: "BookingId",
                principalTable: "tbl_booking",
                principalColumn: "BookingId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
