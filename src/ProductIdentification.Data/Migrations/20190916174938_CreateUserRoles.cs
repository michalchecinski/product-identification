using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;
using ProductIdentification.Core.Models;
using ProductIdentification.Core.Models.Roles;

namespace ProductIdentification.Data.Migrations
{
    public partial class CreateUserRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            CreateRole(migrationBuilder, Role.Admin);
            CreateRole(migrationBuilder, Role.Manager);
            CreateRole(migrationBuilder, Role.DataManager);
            CreateRole(migrationBuilder, Role.WarehouseMan);
        }

        private static void CreateRole(MigrationBuilder migrationBuilder, string roleName)
        {
            var role = new IdentityRole(roleName);
            migrationBuilder.InsertData("AspNetRoles",
                new[]
                {
                    nameof(IdentityRole.Id), nameof(IdentityRole.Name), nameof(IdentityRole.NormalizedName),
                    nameof(IdentityRole.ConcurrencyStamp)
                },
                new object[] {role.Id, role.Name, role.Name.Normalize(), role.ConcurrencyStamp});
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}