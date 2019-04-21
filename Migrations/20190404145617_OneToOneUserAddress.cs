using Microsoft.EntityFrameworkCore.Migrations;

namespace BabyStore.Migrations
{
    public partial class OneToOneUserAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Address_AddressID",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_AddressID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AddressID",
                table: "AspNetUsers");

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

            migrationBuilder.AddColumn<int>(
                name: "AddressID",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AddressID",
                table: "AspNetUsers",
                column: "AddressID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Address_AddressID",
                table: "AspNetUsers",
                column: "AddressID",
                principalTable: "Address",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
