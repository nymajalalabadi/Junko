using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Junko.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddProductFeatureTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductFeatures",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    FeatureTitle = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    FeatureValue = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductFeatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductFeatures_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "EmailSettings",
                keyColumn: "Id",
                keyValue: 1L,
                column: "LastUpdateDate",
                value: new DateTime(2024, 9, 3, 0, 30, 1, 451, DateTimeKind.Local).AddTicks(2712));

            migrationBuilder.CreateIndex(
                name: "IX_ProductFeatures_ProductId",
                table: "ProductFeatures",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductFeatures");

            migrationBuilder.UpdateData(
                table: "EmailSettings",
                keyColumn: "Id",
                keyValue: 1L,
                column: "LastUpdateDate",
                value: new DateTime(2024, 9, 2, 17, 13, 10, 967, DateTimeKind.Local).AddTicks(4518));
        }
    }
}
