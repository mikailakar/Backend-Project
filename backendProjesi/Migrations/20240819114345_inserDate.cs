using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backendProjesi.Migrations
{
    /// <inheritdoc />
    public partial class inserDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "InserDate",
                table: "Users",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InserDate",
                table: "Users");
        }
    }
}
