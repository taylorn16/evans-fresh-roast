using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Adapter.Data.Migrations
{
    public partial class AddManyToManyContactToEventRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contact_CoffeeRoastingEvents_CoffeeRoastingEventId",
                table: "Contact");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contact",
                table: "Contact");

            migrationBuilder.DropIndex(
                name: "IX_Contact_CoffeeRoastingEventId",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "CoffeeRoastingEventId",
                table: "Contact");

            migrationBuilder.RenameTable(
                name: "Contact",
                newName: "Contacts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contacts",
                table: "Contacts",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "CoffeeRoastingEventContact",
                columns: table => new
                {
                    CoffeeRoastingEventsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContactsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoffeeRoastingEventContact", x => new { x.CoffeeRoastingEventsId, x.ContactsId });
                    table.ForeignKey(
                        name: "FK_CoffeeRoastingEventContact_CoffeeRoastingEvents_CoffeeRoastingEventsId",
                        column: x => x.CoffeeRoastingEventsId,
                        principalTable: "CoffeeRoastingEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CoffeeRoastingEventContact_Contacts_ContactsId",
                        column: x => x.ContactsId,
                        principalTable: "Contacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Order_ContactId",
                table: "Order",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_CoffeeRoastingEventContact_ContactsId",
                table: "CoffeeRoastingEventContact",
                column: "ContactsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Contacts_ContactId",
                table: "Order",
                column: "ContactId",
                principalTable: "Contacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Contacts_ContactId",
                table: "Order");

            migrationBuilder.DropTable(
                name: "CoffeeRoastingEventContact");

            migrationBuilder.DropIndex(
                name: "IX_Order_ContactId",
                table: "Order");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contacts",
                table: "Contacts");

            migrationBuilder.RenameTable(
                name: "Contacts",
                newName: "Contact");

            migrationBuilder.AddColumn<Guid>(
                name: "CoffeeRoastingEventId",
                table: "Contact",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contact",
                table: "Contact",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Contact_CoffeeRoastingEventId",
                table: "Contact",
                column: "CoffeeRoastingEventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contact_CoffeeRoastingEvents_CoffeeRoastingEventId",
                table: "Contact",
                column: "CoffeeRoastingEventId",
                principalTable: "CoffeeRoastingEvents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
