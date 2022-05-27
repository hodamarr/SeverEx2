using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebChatServer.Migrations
{
    public partial class v6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "password",
                table: "User",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "nick",
                table: "User",
                newName: "Nick");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "User",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "image",
                table: "User",
                newName: "Server");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Contact",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contact_UserName",
                table: "Contact",
                column: "UserName");

            migrationBuilder.AddForeignKey(
                name: "FK_Contact_User_UserName",
                table: "Contact",
                column: "UserName",
                principalTable: "User",
                principalColumn: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contact_User_UserName",
                table: "Contact");

            migrationBuilder.DropIndex(
                name: "IX_Contact_UserName",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Contact");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "User",
                newName: "password");

            migrationBuilder.RenameColumn(
                name: "Nick",
                table: "User",
                newName: "nick");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "User",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Server",
                table: "User",
                newName: "image");
        }
    }
}
