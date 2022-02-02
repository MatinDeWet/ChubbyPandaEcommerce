using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChubbyPandaEcommerce.Server.Migrations
{
    public partial class ProductsAddData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "ImageUrl", "Price", "Title" },
                values: new object[,]
                {
                    { 1, "Figurine anime", "https://www.ubuy.za.com/productimg/?image=aHR0cHM6Ly9tLm1lZGlhLWFtYXpvbi5jb20vaW1hZ2VzL0kvNzFZcjErbXRDMkwuX0FDX1NMMTUwMF8uanBn.jpg", 9.99m, "Figur" },
                    { 2, "Figurine anime", "https://m.media-amazon.com/images/I/61CXYX0Qy2L._AC_SY679_.jpg", 9.99m, "Tanya" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
