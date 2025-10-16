using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyGames.Web.Migrations
{
    /// <inheritdoc />
    public partial class OwnerStock_And_ShopStockFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BuyPrice",
                table: "ShopStocks",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SellPrice",
                table: "ShopStocks",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "ShopStocks",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "OwnerStocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false),
                    Qty = table.Column<int>(type: "INTEGER", nullable: false),
                    Source = table.Column<string>(type: "TEXT", nullable: false),
                    BuyPrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    SellPrice = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OwnerStocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OwnerStocks_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OwnerStocks_ProductId",
                table: "OwnerStocks",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OwnerStocks");

            migrationBuilder.DropColumn(
                name: "BuyPrice",
                table: "ShopStocks");

            migrationBuilder.DropColumn(
                name: "SellPrice",
                table: "ShopStocks");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "ShopStocks");
        }
    }
}
