using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Adapter.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Coffee",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OzWeight = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coffee", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CoffeeRoastingEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoffeeRoastingEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CoffeeRoastingEventCoffee",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CoffeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CoffeeRoastingEventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoffeeRoastingEventCoffee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoffeeRoastingEventCoffee_Coffee_CoffeeId",
                        column: x => x.CoffeeId,
                        principalTable: "Coffee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CoffeeRoastingEventCoffee_CoffeeRoastingEvents_CoffeeRoastingEventId",
                        column: x => x.CoffeeRoastingEventId,
                        principalTable: "CoffeeRoastingEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Contact",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CoffeeRoastingEventId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contact", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contact_CoffeeRoastingEvents_CoffeeRoastingEventId",
                        column: x => x.CoffeeRoastingEventId,
                        principalTable: "CoffeeRoastingEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContactId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedTimestamp = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    IsConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    CoffeeRoastingEventId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_CoffeeRoastingEvents_CoffeeRoastingEventId",
                        column: x => x.CoffeeRoastingEventId,
                        principalTable: "CoffeeRoastingEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Invoice",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoice_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderCoffee",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CoffeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderCoffee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderCoffee_Coffee_CoffeeId",
                        column: x => x.CoffeeId,
                        principalTable: "Coffee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderCoffee_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoffeeRoastingEventCoffee_CoffeeId",
                table: "CoffeeRoastingEventCoffee",
                column: "CoffeeId");

            migrationBuilder.CreateIndex(
                name: "IX_CoffeeRoastingEventCoffee_CoffeeRoastingEventId",
                table: "CoffeeRoastingEventCoffee",
                column: "CoffeeRoastingEventId");

            migrationBuilder.CreateIndex(
                name: "IX_Contact_CoffeeRoastingEventId",
                table: "Contact",
                column: "CoffeeRoastingEventId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_OrderId",
                table: "Invoice",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_CoffeeRoastingEventId",
                table: "Order",
                column: "CoffeeRoastingEventId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderCoffee_CoffeeId",
                table: "OrderCoffee",
                column: "CoffeeId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderCoffee_OrderId",
                table: "OrderCoffee",
                column: "OrderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoffeeRoastingEventCoffee");

            migrationBuilder.DropTable(
                name: "Contact");

            migrationBuilder.DropTable(
                name: "Invoice");

            migrationBuilder.DropTable(
                name: "OrderCoffee");

            migrationBuilder.DropTable(
                name: "Coffee");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "CoffeeRoastingEvents");
        }
    }
}
