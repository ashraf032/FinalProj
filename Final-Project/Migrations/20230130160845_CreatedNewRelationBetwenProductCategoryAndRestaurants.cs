using Microsoft.EntityFrameworkCore.Migrations;

namespace Final_Project.Migrations
{
    public partial class CreatedNewRelationBetwenProductCategoryAndRestaurants : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RestuorantId",
                table: "ProductCategories",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_RestuorantId",
                table: "ProductCategories",
                column: "RestuorantId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategories_Restuorants_RestuorantId",
                table: "ProductCategories",
                column: "RestuorantId",
                principalTable: "Restuorants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategories_Restuorants_RestuorantId",
                table: "ProductCategories");

            migrationBuilder.DropIndex(
                name: "IX_ProductCategories_RestuorantId",
                table: "ProductCategories");

            migrationBuilder.DropColumn(
                name: "RestuorantId",
                table: "ProductCategories");
        }
    }
}
