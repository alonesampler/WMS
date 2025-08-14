using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddReceiptItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReceiptItems",
                schema: "WMS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ReceiptDocumentId = table.Column<Guid>(type: "uuid", nullable: false),
                    ResourceId = table.Column<Guid>(type: "uuid", nullable: false),
                    UnitOfMeasureId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiptItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReceiptItems_ReceiptDocuments_ReceiptDocumentId",
                        column: x => x.ReceiptDocumentId,
                        principalSchema: "WMS",
                        principalTable: "ReceiptDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReceiptItems_Resources_ResourceId",
                        column: x => x.ResourceId,
                        principalSchema: "WMS",
                        principalTable: "Resources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReceiptItems_UnitOfMeasures_UnitOfMeasureId",
                        column: x => x.UnitOfMeasureId,
                        principalSchema: "WMS",
                        principalTable: "UnitOfMeasures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ResourceQuantityAggregates",
                schema: "WMS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ResourceId = table.Column<Guid>(type: "uuid", nullable: false),
                    UnitOfMeasureId = table.Column<Guid>(type: "uuid", nullable: false),
                    TotalQuantity = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceQuantityAggregates", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptItems_ReceiptDocumentId",
                schema: "WMS",
                table: "ReceiptItems",
                column: "ReceiptDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptItems_ResourceId",
                schema: "WMS",
                table: "ReceiptItems",
                column: "ResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptItems_UnitOfMeasureId",
                schema: "WMS",
                table: "ReceiptItems",
                column: "UnitOfMeasureId");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceQuantityAggregates_ResourceId_UnitOfMeasureId",
                schema: "WMS",
                table: "ResourceQuantityAggregates",
                columns: new[] { "ResourceId", "UnitOfMeasureId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReceiptItems",
                schema: "WMS");

            migrationBuilder.DropTable(
                name: "ResourceQuantityAggregates",
                schema: "WMS");
        }
    }
}
