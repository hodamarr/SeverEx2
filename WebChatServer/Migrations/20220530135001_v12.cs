using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebChatServer.Migrations
{
    public partial class v12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_Contact_Contactnumber",
                table: "Message");

            migrationBuilder.DropIndex(
                name: "IX_Message_Contactnumber",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "Contactnumber",
                table: "Message");

            migrationBuilder.AlterColumn<int>(
                name: "ContactId",
                table: "Message",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Message_ContactId",
                table: "Message",
                column: "ContactId");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Contact_ContactId",
                table: "Message",
                column: "ContactId",
                principalTable: "Contact",
                principalColumn: "number",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_Contact_ContactId",
                table: "Message");

            migrationBuilder.DropIndex(
                name: "IX_Message_ContactId",
                table: "Message");

            migrationBuilder.AlterColumn<string>(
                name: "ContactId",
                table: "Message",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Contactnumber",
                table: "Message",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Message_Contactnumber",
                table: "Message",
                column: "Contactnumber");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Contact_Contactnumber",
                table: "Message",
                column: "Contactnumber",
                principalTable: "Contact",
                principalColumn: "number");
        }
    }
}
