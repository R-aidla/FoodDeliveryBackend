using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FoodDeliveryBackend.Migrations
{
    /// <inheritdoc />
    public partial class ImplementDeliveryRules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Cities_DestinationCityId",
                table: "Order");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.RenameColumn(
                name: "DestinationCityId",
                table: "Order",
                newName: "DeliveryRuleId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_DestinationCityId",
                table: "Order",
                newName: "IX_Order_DeliveryRuleId");

            migrationBuilder.CreateTable(
                name: "DeliveryRegionRules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RegionName = table.Column<string>(type: "TEXT", nullable: false),
                    BaseCarCost = table.Column<decimal>(type: "TEXT", nullable: false),
                    BaseBikeCost = table.Column<decimal>(type: "TEXT", nullable: false),
                    BaseScooterCost = table.Column<decimal>(type: "TEXT", nullable: false),
                    SnowyWeatherCost = table.Column<decimal>(type: "TEXT", nullable: false),
                    RainyWeatherCost = table.Column<decimal>(type: "TEXT", nullable: false),
                    MaxLowTemperatureCost = table.Column<decimal>(type: "TEXT", nullable: false),
                    MinLowTemperatureCost = table.Column<decimal>(type: "TEXT", nullable: false),
                    HighWindsCost = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryRegionRules", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "DeliveryRegionRules",
                columns: new[] { "Id", "BaseBikeCost", "BaseCarCost", "BaseScooterCost", "HighWindsCost", "MaxLowTemperatureCost", "MinLowTemperatureCost", "RainyWeatherCost", "RegionName", "SnowyWeatherCost" },
                values: new object[,]
                {
                    { 1, 3m, 4m, 3.5m, 0.5m, 1m, 0.5m, 0.5m, "Tallinn", 1m },
                    { 2, 2.5m, 3.5m, 3m, 0.5m, 1m, 0.5m, 0.5m, "Tartu", 1m },
                    { 3, 2m, 3m, 2.5m, 0.5m, 1m, 0.5m, 0.5m, "Pärnu", 1m }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Order_DeliveryRegionRules_DeliveryRuleId",
                table: "Order",
                column: "DeliveryRuleId",
                principalTable: "DeliveryRegionRules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_DeliveryRegionRules_DeliveryRuleId",
                table: "Order");

            migrationBuilder.DropTable(
                name: "DeliveryRegionRules");

            migrationBuilder.RenameColumn(
                name: "DeliveryRuleId",
                table: "Order",
                newName: "DestinationCityId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_DeliveryRuleId",
                table: "Order",
                newName: "IX_Order_DestinationCityId");

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CityName = table.Column<string>(type: "TEXT", nullable: false),
                    RegionFee = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "CityName", "RegionFee" },
                values: new object[,]
                {
                    { 1, "Tallinn", 3m },
                    { 2, "Tartu", 2.5m },
                    { 3, "Pärnu", 2m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cities_CityName",
                table: "Cities",
                column: "CityName",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Cities_DestinationCityId",
                table: "Order",
                column: "DestinationCityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
