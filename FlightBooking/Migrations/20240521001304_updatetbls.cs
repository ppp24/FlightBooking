using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightBooking.Migrations
{
    public partial class updatetbls : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CVV",
                table: "TblPayments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CardNumber",
                table: "TblPayments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ExpiryMonth",
                table: "TblPayments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ExpiryYear",
                table: "TblPayments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "NameOnCard",
                table: "TblPayments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte[]>(
                name: "attachment",
                table: "tbl_passengerdetails",
                type: "varbinary(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CVV",
                table: "TblPayments");

            migrationBuilder.DropColumn(
                name: "CardNumber",
                table: "TblPayments");

            migrationBuilder.DropColumn(
                name: "ExpiryMonth",
                table: "TblPayments");

            migrationBuilder.DropColumn(
                name: "ExpiryYear",
                table: "TblPayments");

            migrationBuilder.DropColumn(
                name: "NameOnCard",
                table: "TblPayments");

            migrationBuilder.DropColumn(
                name: "attachment",
                table: "tbl_passengerdetails");
        }
    }
}
