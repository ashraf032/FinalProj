using Microsoft.EntityFrameworkCore.Migrations;

namespace Final_Project.Migrations
{
    public partial class categiriesnullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Restuorants_RestuorantId",
                table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "StoretId",
                table: "Products",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "RestuorantId",
                table: "Products",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Restuorants_RestuorantId",
                table: "Products",
                column: "RestuorantId",
                principalTable: "Restuorants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Restuorants_RestuorantId",
                table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "StoretId",
                table: "Products",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RestuorantId",
                table: "Products",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Restuorants_RestuorantId",
                table: "Products",
                column: "RestuorantId",
                principalTable: "Restuorants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
