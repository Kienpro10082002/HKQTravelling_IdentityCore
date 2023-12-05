using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HKQTravellingAuthenication.Data.Migrations
{
    /// <inheritdoc />
    public partial class tourtypeandinforofappuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfInssuance",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Users",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NewCitizenIdentification",
                table: "Users",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OldCitizenIdentification",
                table: "Users",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "tourTypes",
                columns: table => new
                {
                    TYPE_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TYPE_NAME = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TOUR_ID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tourTypes", x => x.TYPE_ID);
                    table.ForeignKey(
                        name: "FK_tourTypes_tours_TOUR_ID",
                        column: x => x.TOUR_ID,
                        principalTable: "tours",
                        principalColumn: "TOUR_ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_NewCitizenIdentification",
                table: "Users",
                column: "NewCitizenIdentification",
                unique: true,
                filter: "[NewCitizenIdentification] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_OldCitizenIdentification",
                table: "Users",
                column: "OldCitizenIdentification",
                unique: true,
                filter: "[OldCitizenIdentification] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_tourTypes_TOUR_ID",
                table: "tourTypes",
                column: "TOUR_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tourTypes");

            migrationBuilder.DropIndex(
                name: "IX_Users_NewCitizenIdentification",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_OldCitizenIdentification",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DateOfInssuance",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "NewCitizenIdentification",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "OldCitizenIdentification",
                table: "Users");
        }
    }
}
