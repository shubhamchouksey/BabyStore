using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BabyStore.Migrations
{
    public partial class addOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "BasketLine",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    OrderID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserID = table.Column<string>(nullable: true),
                    DeliveryName = table.Column<string>(nullable: true),
                    DeliveryAddressID = table.Column<int>(nullable: true),
                    TotalPrice = table.Column<decimal>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.OrderID);
                    table.ForeignKey(
                        name: "FK_Order_Address_DeliveryAddressID",
                        column: x => x.DeliveryAddressID,
                        principalTable: "Address",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderLine",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrderID = table.Column<int>(nullable: false),
                    ProductID = table.Column<int>(nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    ProductName = table.Column<string>(nullable: true),
                    UnitPrice = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderLine", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OrderLine_Order_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Order",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderLine_Product_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Product",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Order_DeliveryAddressID",
                table: "Order",
                column: "DeliveryAddressID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderLine_OrderID",
                table: "OrderLine",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderLine_ProductID",
                table: "OrderLine",
                column: "ProductID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderLine");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.AlterColumn<int>(
                name: "DateCreated",
                table: "BasketLine",
                nullable: false,
                oldClrType: typeof(DateTime));
        }
    }
}
