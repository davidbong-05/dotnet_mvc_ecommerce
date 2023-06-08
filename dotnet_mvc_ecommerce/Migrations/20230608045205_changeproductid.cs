using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dotnet_mvc_ecommerce.Migrations
{
    /// <inheritdoc />
    public partial class changeproductid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "Product",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Product",
                newName: "id");
        }
    }
}
