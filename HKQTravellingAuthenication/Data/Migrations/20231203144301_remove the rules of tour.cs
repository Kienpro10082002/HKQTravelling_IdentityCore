using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HKQTravellingAuthenication.Data.Migrations
{
    /// <inheritdoc />
    public partial class removetherulesoftour : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_Users_AuthorId",
                table: "Post");

            migrationBuilder.DropTable(
                name: "rules");

            migrationBuilder.AddColumn<string>(
                name: "CANCLE_CHANGE",
                table: "tours",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NOTE",
                table: "tours",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PRICE_INCLUDE",
                table: "tours",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PRICE_NOT_INCLUDE",
                table: "tours",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SURCHARGE",
                table: "tours",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "Post",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Users_AuthorId",
                table: "Post",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_Users_AuthorId",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "CANCLE_CHANGE",
                table: "tours");

            migrationBuilder.DropColumn(
                name: "NOTE",
                table: "tours");

            migrationBuilder.DropColumn(
                name: "PRICE_INCLUDE",
                table: "tours");

            migrationBuilder.DropColumn(
                name: "PRICE_NOT_INCLUDE",
                table: "tours");

            migrationBuilder.DropColumn(
                name: "SURCHARGE",
                table: "tours");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "Post",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "rules",
                columns: table => new
                {
                    TourId = table.Column<long>(type: "bigint", nullable: false),
                    CANCLE_CHANGE = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    NOTE = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PRICE_INCLUDE = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PRICE_NOT_INCLUDE = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SURCHARGE = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rules", x => x.TourId);
                    table.ForeignKey(
                        name: "FK_rules_tours_TourId",
                        column: x => x.TourId,
                        principalTable: "tours",
                        principalColumn: "TOUR_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Users_AuthorId",
                table: "Post",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
