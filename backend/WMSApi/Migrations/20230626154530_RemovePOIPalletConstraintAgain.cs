using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMSApi.Migrations
{
    /// <inheritdoc />
    public partial class RemovePOIPalletConstraintAgain : Migration
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
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderItems_Pallets_PalletId",
                table: "PurchaseOrderItems",
                column: "PalletId",
                principalTable: "Pallets",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderItems_Pallets_PalletId",
                table: "PurchaseOrderItems",
                column: "PalletId",
                principalTable: "Pallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
