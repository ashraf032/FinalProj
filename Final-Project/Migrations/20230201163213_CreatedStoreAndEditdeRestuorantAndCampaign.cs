using Microsoft.EntityFrameworkCore.Migrations;

namespace Final_Project.Migrations
{
    public partial class CreatedStoreAndEditdeRestuorantAndCampaign : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Campaign",
                table: "Restuorants");

            migrationBuilder.AddColumn<int>(
                name: "CampaignId",
                table: "Restuorants",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Restuorants",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeliveryFree",
                table: "Restuorants",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Restuorants",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WorkTime",
                table: "Restuorants",
                maxLength: 15,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "StoreId",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StoreId",
                table: "ProductCategories",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StoreId",
                table: "BasketItems",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Campaigns",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CampaignPercent = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campaigns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stores",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 30, nullable: false),
                    Title = table.Column<string>(maxLength: 30, nullable: false),
                    Image = table.Column<string>(nullable: true),
                    Adress = table.Column<string>(maxLength: 100, nullable: false),
                    Description = table.Column<string>(maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(maxLength: 13, nullable: false),
                    WorkTime = table.Column<string>(maxLength: 15, nullable: false),
                    IsCampaign = table.Column<bool>(nullable: false),
                    IsDeliveryFree = table.Column<bool>(nullable: false),
                    AppUserId = table.Column<string>(nullable: true),
                    CampaignId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stores_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Restuorants_CampaignId",
                table: "Restuorants",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_StoreId",
                table: "Products",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_StoreId",
                table: "ProductCategories",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_BasketItems_StoreId",
                table: "BasketItems",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Stores_CampaignId",
                table: "Stores",
                column: "CampaignId");

            migrationBuilder.AddForeignKey(
                name: "FK_BasketItems_Stores_StoreId",
                table: "BasketItems",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategories_Stores_StoreId",
                table: "ProductCategories",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Stores_StoreId",
                table: "Products",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Restuorants_Campaigns_CampaignId",
                table: "Restuorants",
                column: "CampaignId",
                principalTable: "Campaigns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasketItems_Stores_StoreId",
                table: "BasketItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategories_Stores_StoreId",
                table: "ProductCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Stores_StoreId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Restuorants_Campaigns_CampaignId",
                table: "Restuorants");

            migrationBuilder.DropTable(
                name: "Stores");

            migrationBuilder.DropTable(
                name: "Campaigns");

            migrationBuilder.DropIndex(
                name: "IX_Restuorants_CampaignId",
                table: "Restuorants");

            migrationBuilder.DropIndex(
                name: "IX_Products_StoreId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_ProductCategories_StoreId",
                table: "ProductCategories");

            migrationBuilder.DropIndex(
                name: "IX_BasketItems_StoreId",
                table: "BasketItems");

            migrationBuilder.DropColumn(
                name: "CampaignId",
                table: "Restuorants");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Restuorants");

            migrationBuilder.DropColumn(
                name: "IsDeliveryFree",
                table: "Restuorants");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Restuorants");

            migrationBuilder.DropColumn(
                name: "WorkTime",
                table: "Restuorants");

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "ProductCategories");

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "BasketItems");

            migrationBuilder.AddColumn<int>(
                name: "Campaign",
                table: "Restuorants",
                type: "int",
                nullable: true);
        }
    }
}
