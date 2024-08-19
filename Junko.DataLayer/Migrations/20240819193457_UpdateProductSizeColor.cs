using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Junko.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductSizeColor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductSelectedColorSizes");

            migrationBuilder.AddColumn<long>(
                name: "ProductId",
                table: "ProductSizes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ProductId",
                table: "ProductColors",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.UpdateData(
                table: "EmailSettings",
                keyColumn: "Id",
                keyValue: 1L,
                column: "LastUpdateDate",
                value: new DateTime(2024, 8, 19, 23, 4, 56, 818, DateTimeKind.Local).AddTicks(3100));

            migrationBuilder.CreateIndex(
                name: "IX_ProductSizes_ProductId",
                table: "ProductSizes",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductColors_ProductId",
                table: "ProductColors",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductColors_Products_ProductId",
                table: "ProductColors",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSizes_Products_ProductId",
                table: "ProductSizes",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductColors_Products_ProductId",
                table: "ProductColors");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSizes_Products_ProductId",
                table: "ProductSizes");

            migrationBuilder.DropIndex(
                name: "IX_ProductSizes_ProductId",
                table: "ProductSizes");

            migrationBuilder.DropIndex(
                name: "IX_ProductColors_ProductId",
                table: "ProductColors");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ProductSizes");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ProductColors");

            migrationBuilder.CreateTable(
                name: "ProductSelectedColorSizes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductColorId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    ProductSizeId = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSelectedColorSizes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductSelectedColorSizes_ProductColors_ProductColorId",
                        column: x => x.ProductColorId,
                        principalTable: "ProductColors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductSelectedColorSizes_ProductSizes_ProductSizeId",
                        column: x => x.ProductSizeId,
                        principalTable: "ProductSizes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductSelectedColorSizes_Products_ProductId",
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
                value: new DateTime(2024, 8, 19, 22, 35, 59, 833, DateTimeKind.Local).AddTicks(1630));

            migrationBuilder.CreateIndex(
                name: "IX_ProductSelectedColorSizes_ProductColorId",
                table: "ProductSelectedColorSizes",
                column: "ProductColorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSelectedColorSizes_ProductId",
                table: "ProductSelectedColorSizes",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSelectedColorSizes_ProductSizeId",
                table: "ProductSelectedColorSizes",
                column: "ProductSizeId");
        }
    }
}
