using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Junko.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSizeProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSelectedColorSize_ProductColor_ProductColorId",
                table: "ProductSelectedColorSize");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSelectedColorSize_ProductSize_ProductSizeId",
                table: "ProductSelectedColorSize");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSelectedColorSize_Products_ProductId",
                table: "ProductSelectedColorSize");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductSize",
                table: "ProductSize");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductSelectedColorSize",
                table: "ProductSelectedColorSize");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductColor",
                table: "ProductColor");

            migrationBuilder.DropColumn(
                name: "Count",
                table: "Products");

            migrationBuilder.RenameTable(
                name: "ProductSize",
                newName: "ProductSizes");

            migrationBuilder.RenameTable(
                name: "ProductSelectedColorSize",
                newName: "ProductSelectedColorSizes");

            migrationBuilder.RenameTable(
                name: "ProductColor",
                newName: "ProductColors");

            migrationBuilder.RenameIndex(
                name: "IX_ProductSelectedColorSize_ProductSizeId",
                table: "ProductSelectedColorSizes",
                newName: "IX_ProductSelectedColorSizes_ProductSizeId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductSelectedColorSize_ProductId",
                table: "ProductSelectedColorSizes",
                newName: "IX_ProductSelectedColorSizes_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductSelectedColorSize_ProductColorId",
                table: "ProductSelectedColorSizes",
                newName: "IX_ProductSelectedColorSizes_ProductColorId");

            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "ProductSizes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductSizes",
                table: "ProductSizes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductSelectedColorSizes",
                table: "ProductSelectedColorSizes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductColors",
                table: "ProductColors",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "EmailSettings",
                keyColumn: "Id",
                keyValue: 1L,
                column: "LastUpdateDate",
                value: new DateTime(2024, 8, 19, 22, 35, 59, 833, DateTimeKind.Local).AddTicks(1630));

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSelectedColorSizes_ProductColors_ProductColorId",
                table: "ProductSelectedColorSizes",
                column: "ProductColorId",
                principalTable: "ProductColors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSelectedColorSizes_ProductSizes_ProductSizeId",
                table: "ProductSelectedColorSizes",
                column: "ProductSizeId",
                principalTable: "ProductSizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSelectedColorSizes_Products_ProductId",
                table: "ProductSelectedColorSizes",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSelectedColorSizes_ProductColors_ProductColorId",
                table: "ProductSelectedColorSizes");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSelectedColorSizes_ProductSizes_ProductSizeId",
                table: "ProductSelectedColorSizes");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSelectedColorSizes_Products_ProductId",
                table: "ProductSelectedColorSizes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductSizes",
                table: "ProductSizes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductSelectedColorSizes",
                table: "ProductSelectedColorSizes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductColors",
                table: "ProductColors");

            migrationBuilder.DropColumn(
                name: "Count",
                table: "ProductSizes");

            migrationBuilder.RenameTable(
                name: "ProductSizes",
                newName: "ProductSize");

            migrationBuilder.RenameTable(
                name: "ProductSelectedColorSizes",
                newName: "ProductSelectedColorSize");

            migrationBuilder.RenameTable(
                name: "ProductColors",
                newName: "ProductColor");

            migrationBuilder.RenameIndex(
                name: "IX_ProductSelectedColorSizes_ProductSizeId",
                table: "ProductSelectedColorSize",
                newName: "IX_ProductSelectedColorSize_ProductSizeId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductSelectedColorSizes_ProductId",
                table: "ProductSelectedColorSize",
                newName: "IX_ProductSelectedColorSize_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductSelectedColorSizes_ProductColorId",
                table: "ProductSelectedColorSize",
                newName: "IX_ProductSelectedColorSize_ProductColorId");

            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductSize",
                table: "ProductSize",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductSelectedColorSize",
                table: "ProductSelectedColorSize",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductColor",
                table: "ProductColor",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "EmailSettings",
                keyColumn: "Id",
                keyValue: 1L,
                column: "LastUpdateDate",
                value: new DateTime(2024, 8, 15, 11, 17, 32, 792, DateTimeKind.Local).AddTicks(8080));

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSelectedColorSize_ProductColor_ProductColorId",
                table: "ProductSelectedColorSize",
                column: "ProductColorId",
                principalTable: "ProductColor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSelectedColorSize_ProductSize_ProductSizeId",
                table: "ProductSelectedColorSize",
                column: "ProductSizeId",
                principalTable: "ProductSize",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSelectedColorSize_Products_ProductId",
                table: "ProductSelectedColorSize",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
