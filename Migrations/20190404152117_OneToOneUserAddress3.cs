using Microsoft.EntityFrameworkCore.Migrations;

namespace BabyStore.Migrations
{
    public partial class OneToOneUserAddress3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserRef",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserRef",
                table: "AspNetUsers",
                column: "UserRef",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Address_UserRef",
                table: "AspNetUsers",
                column: "UserRef",
                principalTable: "Address",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Address_UserRef",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UserRef",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserRef",
                table: "AspNetUsers");
        }
    }
}
