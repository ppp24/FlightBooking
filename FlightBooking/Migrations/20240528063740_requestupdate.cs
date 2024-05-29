using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightBooking.Migrations
{
    public partial class requestupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EditPlan",
                table: "tbl_requests",
                newName: "Comment");

            migrationBuilder.RenameColumn(
                name: "ApplyLoyaltyProgram",
                table: "tbl_requests",
                newName: "Action");

            migrationBuilder.AlterColumn<string>(
                name: "UploadDocuments",
                table: "tbl_requests",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Comment",
                table: "tbl_requests",
                newName: "EditPlan");

            migrationBuilder.RenameColumn(
                name: "Action",
                table: "tbl_requests",
                newName: "ApplyLoyaltyProgram");

            migrationBuilder.AlterColumn<string>(
                name: "UploadDocuments",
                table: "tbl_requests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
