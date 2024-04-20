using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel.ATR.Portal.Migrations
{
    public partial class AddPathToImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PathToImage",
                table: "Rooms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PathToImage",
                table: "Rooms");
        }
    }
}
