using Microsoft.EntityFrameworkCore.Migrations;

namespace BugZapper.Migrations
{
    public partial class SecondCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PermissionLevel",
                table: "User",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BugDescription",
                table: "Ticket",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClosedDate",
                table: "Ticket",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TicketOwner",
                table: "Ticket",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TicketStatus",
                table: "Ticket",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TicketSubject",
                table: "Ticket",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PermissionLevel",
                table: "User");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "User");

            migrationBuilder.DropColumn(
                name: "BugDescription",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "ClosedDate",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "TicketOwner",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "TicketStatus",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "TicketSubject",
                table: "Ticket");
        }
    }
}
