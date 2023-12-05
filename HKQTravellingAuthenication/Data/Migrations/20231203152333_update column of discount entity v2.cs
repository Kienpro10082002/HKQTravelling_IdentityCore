using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HKQTravellingAuthenication.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatecolumnofdiscountentityv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DIS_DATE_End",
                table: "discounts",
                newName: "DIS_DATE_END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DIS_DATE_END",
                table: "discounts",
                newName: "DIS_DATE_End");
        }
    }
}
