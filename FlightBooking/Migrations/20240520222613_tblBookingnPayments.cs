using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightBooking.Migrations
{
    public partial class tblBookingnPayments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_booking_AspNetUsers_UserId",
                table: "tbl_booking");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_booking_tbl_flight_FlightId",
                table: "tbl_booking");

            migrationBuilder.DropIndex(
                name: "IX_tbl_booking_UserId",
                table: "tbl_booking");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "tbl_booking");

            migrationBuilder.RenameColumn(
                name: "FlightId",
                table: "tbl_booking",
                newName: "PassengerId");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_booking_FlightId",
                table: "tbl_booking",
                newName: "IX_tbl_booking_PassengerId");

            migrationBuilder.AlterColumn<int>(
                name: "LoyalityPoints",
                table: "tbl_passengerdetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateAdded",
                table: "tbl_passengerdetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "BookingDate",
                table: "tbl_booking",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "OutboundFlightId",
                table: "tbl_booking",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PaymentStatus",
                table: "tbl_booking",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ReturnFlightId",
                table: "tbl_booking",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SpecialRequests",
                table: "tbl_booking",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "tbl_booking",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "TblPayments",
                columns: table => new
                {
                    PaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblPayments", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_TblPayments_tbl_booking_BookingId",
                        column: x => x.BookingId,
                        principalTable: "tbl_booking",
                        principalColumn: "BookingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_booking_OutboundFlightId",
                table: "tbl_booking",
                column: "OutboundFlightId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_booking_ReturnFlightId",
                table: "tbl_booking",
                column: "ReturnFlightId");

            migrationBuilder.CreateIndex(
                name: "IX_TblPayments_BookingId",
                table: "TblPayments",
                column: "BookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_booking_tbl_flight_OutboundFlightId",
                table: "tbl_booking",
                column: "OutboundFlightId",
                principalTable: "tbl_flight",
                principalColumn: "FlightId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_booking_tbl_flight_ReturnFlightId",
                table: "tbl_booking",
                column: "ReturnFlightId",
                principalTable: "tbl_flight",
                principalColumn: "FlightId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_booking_tbl_passengerdetails_PassengerId",
                table: "tbl_booking",
                column: "PassengerId",
                principalTable: "tbl_passengerdetails",
                principalColumn: "PassengerId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_booking_tbl_flight_OutboundFlightId",
                table: "tbl_booking");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_booking_tbl_flight_ReturnFlightId",
                table: "tbl_booking");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_booking_tbl_passengerdetails_PassengerId",
                table: "tbl_booking");

            migrationBuilder.DropTable(
                name: "TblPayments");

            migrationBuilder.DropIndex(
                name: "IX_tbl_booking_OutboundFlightId",
                table: "tbl_booking");

            migrationBuilder.DropIndex(
                name: "IX_tbl_booking_ReturnFlightId",
                table: "tbl_booking");

            migrationBuilder.DropColumn(
                name: "DateAdded",
                table: "tbl_passengerdetails");

            migrationBuilder.DropColumn(
                name: "BookingDate",
                table: "tbl_booking");

            migrationBuilder.DropColumn(
                name: "OutboundFlightId",
                table: "tbl_booking");

            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "tbl_booking");

            migrationBuilder.DropColumn(
                name: "ReturnFlightId",
                table: "tbl_booking");

            migrationBuilder.DropColumn(
                name: "SpecialRequests",
                table: "tbl_booking");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "tbl_booking");

            migrationBuilder.RenameColumn(
                name: "PassengerId",
                table: "tbl_booking",
                newName: "FlightId");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_booking_PassengerId",
                table: "tbl_booking",
                newName: "IX_tbl_booking_FlightId");

            migrationBuilder.AlterColumn<int>(
                name: "LoyalityPoints",
                table: "tbl_passengerdetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "tbl_booking",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_booking_UserId",
                table: "tbl_booking",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_booking_AspNetUsers_UserId",
                table: "tbl_booking",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_booking_tbl_flight_FlightId",
                table: "tbl_booking",
                column: "FlightId",
                principalTable: "tbl_flight",
                principalColumn: "FlightId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
