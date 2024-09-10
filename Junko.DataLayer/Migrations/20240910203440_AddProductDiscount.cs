using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Junko.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddProductDiscount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductDiscounts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    Percentage = table.Column<int>(type: "int", nullable: false),
                    ExpireDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DiscountNumber = table.Column<int>(type: "int", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDiscounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductDiscounts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "productDiscountUses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductDiscountId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productDiscountUses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_productDiscountUses_ProductDiscounts_ProductDiscountId",
                        column: x => x.ProductDiscountId,
                        principalTable: "ProductDiscounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_productDiscountUses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "EmailSettings",
                keyColumn: "Id",
                keyValue: 1L,
                column: "LastUpdateDate",
                value: new DateTime(2024, 9, 11, 0, 4, 39, 473, DateTimeKind.Local).AddTicks(4368));

            migrationBuilder.CreateIndex(
                name: "IX_ProductDiscounts_ProductId",
                table: "ProductDiscounts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_productDiscountUses_ProductDiscountId",
                table: "productDiscountUses",
                column: "ProductDiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_productDiscountUses_UserId",
                table: "productDiscountUses",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "productDiscountUses");

            migrationBuilder.DropTable(
                name: "ProductDiscounts");

            migrationBuilder.UpdateData(
                table: "EmailSettings",
                keyColumn: "Id",
                keyValue: 1L,
                column: "LastUpdateDate",
                value: new DateTime(2024, 9, 9, 17, 52, 25, 595, DateTimeKind.Local).AddTicks(3781));
        }
    }
}
