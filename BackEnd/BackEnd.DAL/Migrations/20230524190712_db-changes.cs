using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackEnd.DAL.Migrations
{
    public partial class dbchanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Altitude",
                table: "Aircrafts");

            migrationBuilder.DropColumn(
                name: "Heading",
                table: "Aircrafts");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Aircrafts");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Aircrafts");

            migrationBuilder.RenameColumn(
                name: "AircraftType",
                table: "Aircrafts",
                newName: "CallSign");

            migrationBuilder.CreateTable(
                name: "AircraftLocations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AircraftType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitude = table.Column<float>(type: "real", nullable: false),
                    Longitude = table.Column<float>(type: "real", nullable: false),
                    Altitude = table.Column<int>(type: "int", nullable: false),
                    Heading = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AircraftLocations", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AircraftLocations");

            migrationBuilder.RenameColumn(
                name: "CallSign",
                table: "Aircrafts",
                newName: "AircraftType");

            migrationBuilder.AddColumn<int>(
                name: "Altitude",
                table: "Aircrafts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Heading",
                table: "Aircrafts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "Latitude",
                table: "Aircrafts",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Longitude",
                table: "Aircrafts",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
