using Microsoft.EntityFrameworkCore.Migrations;

namespace BabyStore.Migrations
{
    public partial class OneToOneUserAddress1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressID",
                table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
