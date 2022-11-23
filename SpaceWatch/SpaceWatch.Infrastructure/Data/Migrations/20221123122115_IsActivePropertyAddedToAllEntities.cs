using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpaceWatch.Infrastructure.Data.Migrations
{
    public partial class IsActivePropertyAddedToAllEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "UserComments",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Content",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "CategoryItems",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.UpdateData(
              table: "Categories",
              keyColumn: "Id",
              keyValue: 1,
              column: "IsActive",
              value: true);

            migrationBuilder.UpdateData(
             table: "Categories",
             keyColumn: "Id",
             keyValue: 2,
             column: "IsActive",
             value: true);

            migrationBuilder.UpdateData(
             table: "Categories",
             keyColumn: "Id",
             keyValue: 3,
             column: "IsActive",
             value: true);

            migrationBuilder.UpdateData(
            table: "CategoryItems",
            keyColumn: "Id",
            keyValue: 1,
            column: "IsActive",
            value: true);

            migrationBuilder.UpdateData(
            table: "CategoryItems",
            keyColumn: "Id",
            keyValue: 2,
            column: "IsActive",
            value: true);

            migrationBuilder.UpdateData(
            table: "Content",
            keyColumn: "Id",
            keyValue: 1,
            column: "IsActive",
            value: true);

            migrationBuilder.UpdateData(
           table: "Content",
           keyColumn: "Id",
           keyValue: 2,
           column: "IsActive",
           value: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "UserComments");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Content");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "CategoryItems");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Categories");
        }
    }
}
