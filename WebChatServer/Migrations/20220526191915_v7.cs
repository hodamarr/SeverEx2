using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebChatServer.Migrations
{
    public partial class v7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contact_User_UserName",
                table: "Contact");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Contact",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Contact_User_UserName",
                table: "Contact",
                column: "UserName",
                principalTable: "User",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contact_User_UserName",
                table: "Contact");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Contact",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Contact_User_UserName",
                table: "Contact",
                column: "UserName",
                principalTable: "User",
                principalColumn: "Name");
        }
    }
}
