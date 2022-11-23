using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpaceWatch.Infrastructure.Data.Migrations
{
    public partial class IsActivePropertyAddedToMediaTypeEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "MediaTypes",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "MediaTypes");
        }
    }
}
