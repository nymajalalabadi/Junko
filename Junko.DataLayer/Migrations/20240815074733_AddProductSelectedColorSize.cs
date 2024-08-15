using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Junko.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddProductSelectedColorSize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductColor_Products_ProductId",
                table: "ProductColor");

            migrationBuilder.DropIndex(
                name: "IX_ProductColor_ProductId",
                table: "ProductColor");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ProductColor");

            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Price",
                table: "ProductColor",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "ProductSize",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Size = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSize", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductSelectedColorSize",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    ProductColorId = table.Column<long>(type: "bigint", nullable: false),
                    ProductSizeId = table.Column<long>(type: "bigint", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSelectedColorSize", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductSelectedColorSize_ProductColor_ProductColorId",
                        column: x => x.ProductColorId,
                        principalTable: "ProductColor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductSelectedColorSize_ProductSize_ProductSizeId",
                        column: x => x.ProductSizeId,
                        principalTable: "ProductSize",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductSelectedColorSize_Products_ProductId",
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
                value: new DateTime(2024, 8, 15, 11, 17, 32, 792, DateTimeKind.Local).AddTicks(8080));

            migrationBuilder.CreateIndex(
                name: "IX_ProductSelectedColorSize_ProductColorId",
                table: "ProductSelectedColorSize",
                column: "ProductColorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSelectedColorSize_ProductId",
                table: "ProductSelectedColorSize",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSelectedColorSize_ProductSizeId",
                table: "ProductSelectedColorSize",
                column: "ProductSizeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductSelectedColorSize");

            migrationBuilder.DropTable(
                name: "ProductSize");

            migrationBuilder.DropColumn(
                name: "Count",
                table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "Price",
                table: "ProductColor",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ProductId",
                table: "ProductColor",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.UpdateData(
                table: "EmailSettings",
                keyColumn: "Id",
                keyValue: 1L,
                column: "LastUpdateDate",
                value: new DateTime(2024, 8, 15, 10, 47, 2, 638, DateTimeKind.Local).AddTicks(6488));

            migrationBuilder.CreateIndex(
                name: "IX_ProductColor_ProductId",
                table: "ProductColor",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductColor_Products_ProductId",
                table: "ProductColor",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
