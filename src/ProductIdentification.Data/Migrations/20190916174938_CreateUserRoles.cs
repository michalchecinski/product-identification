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
            var rolesSql = string.Empty;
            rolesSql += CreateRole(Role.Admin);
            rolesSql += CreateRole(Role.Manager);
            rolesSql += CreateRole(Role.DataManager);
            rolesSql += CreateRole(Role.WarehouseMan);

            migrationBuilder.Sql(rolesSql);
        }

        private static string CreateRole(string roleName)
        {
            var role = new IdentityRole(roleName);
            return $@"INSERT INTO [dbo].[AspNetRoles]
            (Name, NormalizedName, ConcurrencyStamp)
            VALUES( {roleName}, {roleName.Normalize()}, {role.ConcurrencyStamp});";
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var rolesSql = string.Empty;
            rolesSql += DeleteRole(Role.Admin);
            rolesSql += DeleteRole(Role.Manager);
            rolesSql += DeleteRole(Role.DataManager);
            rolesSql += DeleteRole(Role.WarehouseMan);

            migrationBuilder.Sql(rolesSql);
        }
        
        private static string DeleteRole(string roleName)
        {
            return $@"DELETE FROM [dbo].[AspNetRoles] WHERE Name= {roleName};";
        }
    }
}