using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FloralFusion.Domain.Migrations
{
    /// <inheritdoc />
    public partial class MigrateNowhgjd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flowers_FlowerCategories_FlowerCategoryId1",
                table: "Flowers");

            migrationBuilder.DropForeignKey(
                name: "FK_Flowers_FlowerOccasions_FlowerOccasionId1",
                table: "Flowers");

            migrationBuilder.DropIndex(
                name: "IX_Flowers_FlowerCategoryId1",
                table: "Flowers");

            migrationBuilder.DropIndex(
                name: "IX_Flowers_FlowerOccasionId1",
                table: "Flowers");

            migrationBuilder.DropColumn(
                name: "FlowerCategoryId1",
                table: "Flowers");

            migrationBuilder.DropColumn(
                name: "FlowerOccasionId1",
                table: "Flowers");

            migrationBuilder.CreateIndex(
                name: "IX_Flowers_FlowerCategoryId",
                table: "Flowers",
                column: "FlowerCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Flowers_FlowerOccasionId",
                table: "Flowers",
                column: "FlowerOccasionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Flowers_FlowerCategories_FlowerCategoryId",
                table: "Flowers",
                column: "FlowerCategoryId",
                principalTable: "FlowerCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Flowers_FlowerOccasions_FlowerOccasionId",
                table: "Flowers",
                column: "FlowerOccasionId",
                principalTable: "FlowerOccasions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flowers_FlowerCategories_FlowerCategoryId",
                table: "Flowers");

            migrationBuilder.DropForeignKey(
                name: "FK_Flowers_FlowerOccasions_FlowerOccasionId",
                table: "Flowers");

            migrationBuilder.DropIndex(
                name: "IX_Flowers_FlowerCategoryId",
                table: "Flowers");

            migrationBuilder.DropIndex(
                name: "IX_Flowers_FlowerOccasionId",
                table: "Flowers");

            migrationBuilder.AddColumn<long>(
                name: "FlowerCategoryId1",
                table: "Flowers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "FlowerOccasionId1",
                table: "Flowers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Flowers_FlowerCategoryId1",
                table: "Flowers",
                column: "FlowerCategoryId1");

            migrationBuilder.CreateIndex(
                name: "IX_Flowers_FlowerOccasionId1",
                table: "Flowers",
                column: "FlowerOccasionId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Flowers_FlowerCategories_FlowerCategoryId1",
                table: "Flowers",
                column: "FlowerCategoryId1",
                principalTable: "FlowerCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Flowers_FlowerOccasions_FlowerOccasionId1",
                table: "Flowers",
                column: "FlowerOccasionId1",
                principalTable: "FlowerOccasions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
