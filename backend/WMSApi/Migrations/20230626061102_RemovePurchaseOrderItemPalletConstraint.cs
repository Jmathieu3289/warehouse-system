using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMSApi.Migrations
{
    /// <inheritdoc />
    public partial class RemovePurchaseOrderItemPalletConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrderItems_Pallets_PalletId",
                table: "PurchaseOrderItems");

            migrationBuilder.AlterColumn<long>(
                name: "PalletId",
                table: "PurchaseOrderItems",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Section",
                table: "PalletBays",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Row",
                table: "PalletBays",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Floor",
                table: "PalletBays",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_PalletBays_Row_Section_Floor",
                table: "PalletBays",
                columns: new[] { "Row", "Section", "Floor" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderItems_Pallets_PalletId",
                table: "PurchaseOrderItems",
                column: "PalletId",
                principalTable: "Pallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrderItems_Pallets_PalletId",
                table: "PurchaseOrderItems");

            migrationBuilder.DropIndex(
                name: "IX_PalletBays_Row_Section_Floor",
                table: "PalletBays");

            migrationBuilder.AlterColumn<long>(
                name: "PalletId",
                table: "PurchaseOrderItems",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "Section",
                table: "PalletBays",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Row",
                table: "PalletBays",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Floor",
                table: "PalletBays",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderItems_Pallets_PalletId",
                table: "PurchaseOrderItems",
                column: "PalletId",
                principalTable: "Pallets",
                principalColumn: "Id");
        }
    }
}
