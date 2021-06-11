using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pmat_PI.Migrations
{
    public partial class GOOOOOO : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodPostal",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataRegisto",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ExtensaoCodPostal",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Localidade",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Morada",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "sexo",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodPostal",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DataRegisto",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ExtensaoCodPostal",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Localidade",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Morada",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "sexo",
                table: "AspNetUsers");
        }
    }
}
