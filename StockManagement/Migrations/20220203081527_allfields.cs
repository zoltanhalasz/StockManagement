using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StockManagement.Migrations
{
    public partial class allfields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "Stock",
                table: "Stocks");

            migrationBuilder.AddColumn<string>(
                name: "Batch",
                table: "Stocks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CountryOfOrigin",
                table: "Stocks",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiryDate",
                table: "Stocks",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Item",
                table: "Stocks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LicensePlate",
                table: "Stocks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Stocks",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Quantity",
                table: "Stocks",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Stocks",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Batch",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "CountryOfOrigin",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "ExpiryDate",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "Item",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "LicensePlate",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Stocks");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Stocks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Stock",
                table: "Stocks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
