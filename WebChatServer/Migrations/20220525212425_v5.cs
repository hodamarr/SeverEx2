using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebChatServer.Migrations
{
    public partial class v5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Last",
                table: "Contact",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Lastdate",
                table: "Contact",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Last",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "Lastdate",
                table: "Contact");
        }
    }
}
