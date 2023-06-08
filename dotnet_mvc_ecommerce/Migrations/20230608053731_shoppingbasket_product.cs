using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dotnet_mvc_ecommerce.Migrations
{
    /// <inheritdoc />
    public partial class shoppingbasket_product : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_ShoppingBasket_ShoppingBasketId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_ShoppingBasketId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "ShoppingBasketId",
                table: "Product");

            migrationBuilder.CreateTable(
                name: "ShoppingBasket_Product",
                columns: table => new
                {
                    ShoppingBasketId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingBasket_Product", x => new { x.ShoppingBasketId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_ShoppingBasket_Product_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShoppingBasket_Product_ShoppingBasket_ShoppingBasketId",
                        column: x => x.ShoppingBasketId,
                        principalTable: "ShoppingBasket",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingBasket_Product_ProductId",
                table: "ShoppingBasket_Product",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShoppingBasket_Product");

            migrationBuilder.AddColumn<int>(
                name: "ShoppingBasketId",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Product_ShoppingBasketId",
                table: "Product",
                column: "ShoppingBasketId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_ShoppingBasket_ShoppingBasketId",
                table: "Product",
                column: "ShoppingBasketId",
                principalTable: "ShoppingBasket",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
