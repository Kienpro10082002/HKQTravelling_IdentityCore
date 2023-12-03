using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HKQTravellingAuthenication.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatecolumnofdiscountentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DIS_DATE",
                table: "discounts",
                newName: "DIS_DATE_START");

            migrationBuilder.AddColumn<DateTime>(
                name: "DIS_DATE_End",
                table: "discounts",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DIS_DATE_End",
                table: "discounts");

            migrationBuilder.RenameColumn(
                name: "DIS_DATE_START",
                table: "discounts",
                newName: "DIS_DATE");
        }
    }
}
