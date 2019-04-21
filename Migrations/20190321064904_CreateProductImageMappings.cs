using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BabyStore.Migrations
{
    public partial class CreateProductImageMappings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductImageMapping",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ImageNumber = table.Column<int>(nullable: false),
                    ProductID = table.Column<int>(nullable: false),
                    ProductImageID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImageMapping", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProductImageMapping_Product_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Product",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductImageMapping_ProductImage_ProductImageID",
                        column: x => x.ProductImageID,
                        principalTable: "ProductImage",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductImageMapping_ProductID",
                table: "ProductImageMapping",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImageMapping_ProductImageID",
                table: "ProductImageMapping",
                column: "ProductImageID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductImageMapping");
        }
    }
}
