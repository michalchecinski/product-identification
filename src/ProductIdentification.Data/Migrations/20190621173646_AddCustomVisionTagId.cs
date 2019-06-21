using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductIdentification.Data.Migrations
{
    public partial class AddCustomVisionTagId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CustomVisionTagId",
                table: "Products",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomVisionTagId",
                table: "Products");
        }
    }
}
