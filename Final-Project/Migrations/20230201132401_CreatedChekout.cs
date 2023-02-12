using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Final_Project.Migrations
{
    public partial class CreatedChekout : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Count = table.Column<int>(nullable: false),
                    AppUserId = table.Column<string>(nullable: true),
                    ProductId = table.Column<int>(nullable: false),
                    RestuorantId = table.Column<int>(nullable: true),
                    StoreId = table.Column<int>(nullable: true),
                    ISsaleComlete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Restuorants_RestuorantId",
                        column: x => x.RestuorantId,
                        principalTable: "Restuorants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItems_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppUserId = table.Column<string>(nullable: true),
                    RestuorantId = table.Column<int>(nullable: true),
                    StoreId = table.Column<int>(nullable: true),
                    ProductId = table.Column<int>(nullable: false),
                    IsDelivery = table.Column<bool>(nullable: false),
                    IsAccept = table.Column<bool>(nullable: false),
                    IsOrderComlete = table.Column<bool>(nullable: false),
                    IsCourierFind = table.Column<bool>(nullable: false),
                    IsCard = table.Column<bool>(nullable: false),
                    Owner = table.Column<string>(nullable: true),
                    Cvv = table.Column<int>(nullable: false),
                    CardNumber = table.Column<int>(nullable: false),
                    CardMonth = table.Column<string>(nullable: true),
                    CardYear = table.Column<string>(nullable: true),
                    OrderComleete = table.Column<bool>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    TotalPrice = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Restuorants_RestuorantId",
                        column: x => x.RestuorantId,
                        principalTable: "Restuorants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_AppUserId",
                table: "OrderItems",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_RestuorantId",
                table: "OrderItems",
                column: "RestuorantId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_StoreId",
                table: "OrderItems",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AppUserId",
                table: "Orders",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_RestuorantId",
                table: "Orders",
                column: "RestuorantId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_StoreId",
                table: "Orders",
                column: "StoreId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
