using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backendProjesi.Migrations
{
    /// <inheritdoc />
    public partial class Rol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE GetUsersWithRoles
                AS
                BEGIN
                    SELECT 
                        u.Id, 
                        u.Name, 
                        u.Username, 
                        u.Email,
                        r.RoleName
                    FROM Users u
                    INNER JOIN Rol r ON u.Id = r.UserId
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE GetUsersWithRoles");
        }
    }
}
