using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dotnet_mvc_ecommerce.Migrations
{
    /// <inheritdoc />
    public partial class quantity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "ShoppingBasket");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "ShoppingBasket_Product",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "ShoppingBasket_Product");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "ShoppingBasket",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
