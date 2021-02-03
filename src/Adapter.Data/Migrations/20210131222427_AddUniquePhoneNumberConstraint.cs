using Microsoft.EntityFrameworkCore.Migrations;

namespace Adapter.Data.Migrations
{
    public partial class AddUniquePhoneNumberConstraint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoffeeRoastingEventCoffee_Coffee_CoffeeId",
                table: "CoffeeRoastingEventCoffee");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderCoffee_Coffee_CoffeeId",
                table: "OrderCoffee");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Coffee",
                table: "Coffee");

            migrationBuilder.RenameTable(
                name: "Coffee",
                newName: "Coffees");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Contacts",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Coffees",
                table: "Coffees",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_PhoneNumber",
                table: "Contacts",
                column: "PhoneNumber",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CoffeeRoastingEventCoffee_Coffees_CoffeeId",
                table: "CoffeeRoastingEventCoffee",
                column: "CoffeeId",
                principalTable: "Coffees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderCoffee_Coffees_CoffeeId",
                table: "OrderCoffee",
                column: "CoffeeId",
                principalTable: "Coffees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoffeeRoastingEventCoffee_Coffees_CoffeeId",
                table: "CoffeeRoastingEventCoffee");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderCoffee_Coffees_CoffeeId",
                table: "OrderCoffee");

            migrationBuilder.DropIndex(
                name: "IX_Contacts_PhoneNumber",
                table: "Contacts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Coffees",
                table: "Coffees");

            migrationBuilder.RenameTable(
                name: "Coffees",
                newName: "Coffee");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Contacts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Coffee",
                table: "Coffee",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CoffeeRoastingEventCoffee_Coffee_CoffeeId",
                table: "CoffeeRoastingEventCoffee",
                column: "CoffeeId",
                principalTable: "Coffee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderCoffee_Coffee_CoffeeId",
                table: "OrderCoffee",
                column: "CoffeeId",
                principalTable: "Coffee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
