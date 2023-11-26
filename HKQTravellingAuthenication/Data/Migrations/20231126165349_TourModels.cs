using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HKQTravellingAuthenication.Data.Migrations
{
    /// <inheritdoc />
    public partial class TourModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentCategoryId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Category_Category_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalTable: "Category",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "discounts",
                columns: table => new
                {
                    DIS_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DIS_PER = table.Column<double>(type: "float", nullable: true),
                    DIS_NAME = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DIS_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    STATUS = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_discounts", x => x.DIS_ID);
                });

            migrationBuilder.CreateTable(
                name: "endLocations",
                columns: table => new
                {
                    END_LOCATION_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    END_LOCATION_NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_endLocations", x => x.END_LOCATION_ID);
                });

            migrationBuilder.CreateTable(
                name: "startLocations",
                columns: table => new
                {
                    START_LOCATION_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    START_LOCATION_NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_startLocations", x => x.START_LOCATION_ID);
                });

            migrationBuilder.CreateTable(
                name: "tours",
                columns: table => new
                {
                    TOUR_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TOUR_NAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PRICE = table.Column<int>(type: "int", nullable: true),
                    START_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    END_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    STATUS = table.Column<int>(type: "int", nullable: true),
                    CREATION_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    REMAINING = table.Column<int>(type: "int", nullable: true),
                    DIS_ID = table.Column<long>(type: "bigint", nullable: true),
                    START_LOCATION_ID = table.Column<long>(type: "bigint", nullable: true),
                    END_LOCATION_ID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tours", x => x.TOUR_ID);
                    table.ForeignKey(
                        name: "FK_tours_discounts_DIS_ID",
                        column: x => x.DIS_ID,
                        principalTable: "discounts",
                        principalColumn: "DIS_ID");
                    table.ForeignKey(
                        name: "FK_tours_endLocations_END_LOCATION_ID",
                        column: x => x.END_LOCATION_ID,
                        principalTable: "endLocations",
                        principalColumn: "END_LOCATION_ID");
                    table.ForeignKey(
                        name: "FK_tours_startLocations_START_LOCATION_ID",
                        column: x => x.START_LOCATION_ID,
                        principalTable: "startLocations",
                        principalColumn: "START_LOCATION_ID");
                });

            migrationBuilder.CreateTable(
                name: "bookings",
                columns: table => new
                {
                    BOOKING_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BOOKING_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NUM_ADULTS = table.Column<int>(type: "int", nullable: true),
                    NUM_TODDLERS = table.Column<int>(type: "int", nullable: true),
                    NUM_KIDS = table.Column<int>(type: "int", nullable: true),
                    PRICE_ADULTS = table.Column<double>(type: "float", nullable: true),
                    PRICE_TODDLERS = table.Column<double>(type: "float", nullable: true),
                    PRICE_KIDS = table.Column<double>(type: "float", nullable: true),
                    TOUR_ID = table.Column<long>(type: "bigint", nullable: true),
                    USER_ID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bookings", x => x.BOOKING_ID);
                    table.ForeignKey(
                        name: "FK_bookings_Users_USER_ID",
                        column: x => x.USER_ID,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_bookings_tours_TOUR_ID",
                        column: x => x.TOUR_ID,
                        principalTable: "tours",
                        principalColumn: "TOUR_ID");
                });

            migrationBuilder.CreateTable(
                name: "rules",
                columns: table => new
                {
                    TourId = table.Column<long>(type: "bigint", nullable: false),
                    PRICE_INCLUDE = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PRICE_NOT_INCLUDE = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SURCHARGE = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CANCLE_CHANGE = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    NOTE = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
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

            migrationBuilder.CreateTable(
                name: "tourDays",
                columns: table => new
                {
                    TOUR_DAY_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DAY_NUMBER = table.Column<int>(type: "int", nullable: true),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DESTINATION = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TIME_SCHEDULE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TOUR_ID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tourDays", x => x.TOUR_DAY_ID);
                    table.ForeignKey(
                        name: "FK_tourDays_tours_TOUR_ID",
                        column: x => x.TOUR_ID,
                        principalTable: "tours",
                        principalColumn: "TOUR_ID");
                });

            migrationBuilder.CreateTable(
                name: "tourImages",
                columns: table => new
                {
                    IMAGE_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IMAGE_URL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DAY_NUMBER = table.Column<int>(type: "int", nullable: true),
                    TOUR_ID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tourImages", x => x.IMAGE_ID);
                    table.ForeignKey(
                        name: "FK_tourImages_tours_TOUR_ID",
                        column: x => x.TOUR_ID,
                        principalTable: "tours",
                        principalColumn: "TOUR_ID");
                });

            migrationBuilder.CreateTable(
                name: "payments",
                columns: table => new
                {
                    PAYMENT_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AMOUNT = table.Column<int>(type: "int", nullable: true),
                    TOTAL_PRICE = table.Column<double>(type: "float", nullable: true),
                    PAYMENT_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BOOKING_ID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payments", x => x.PAYMENT_ID);
                    table.ForeignKey(
                        name: "FK_payments_bookings_BOOKING_ID",
                        column: x => x.BOOKING_ID,
                        principalTable: "bookings",
                        principalColumn: "BOOKING_ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_bookings_TOUR_ID",
                table: "bookings",
                column: "TOUR_ID");

            migrationBuilder.CreateIndex(
                name: "IX_bookings_USER_ID",
                table: "bookings",
                column: "USER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Category_ParentCategoryId",
                table: "Category",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Category_Slug",
                table: "Category",
                column: "Slug");

            migrationBuilder.CreateIndex(
                name: "IX_endLocations_END_LOCATION_NAME",
                table: "endLocations",
                column: "END_LOCATION_NAME",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_payments_BOOKING_ID",
                table: "payments",
                column: "BOOKING_ID");

            migrationBuilder.CreateIndex(
                name: "IX_startLocations_START_LOCATION_NAME",
                table: "startLocations",
                column: "START_LOCATION_NAME",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tourDays_TOUR_ID",
                table: "tourDays",
                column: "TOUR_ID");

            migrationBuilder.CreateIndex(
                name: "IX_tourImages_TOUR_ID",
                table: "tourImages",
                column: "TOUR_ID");

            migrationBuilder.CreateIndex(
                name: "IX_tours_DIS_ID",
                table: "tours",
                column: "DIS_ID");

            migrationBuilder.CreateIndex(
                name: "IX_tours_END_LOCATION_ID",
                table: "tours",
                column: "END_LOCATION_ID");

            migrationBuilder.CreateIndex(
                name: "IX_tours_START_LOCATION_ID",
                table: "tours",
                column: "START_LOCATION_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "payments");

            migrationBuilder.DropTable(
                name: "rules");

            migrationBuilder.DropTable(
                name: "tourDays");

            migrationBuilder.DropTable(
                name: "tourImages");

            migrationBuilder.DropTable(
                name: "bookings");

            migrationBuilder.DropTable(
                name: "tours");

            migrationBuilder.DropTable(
                name: "discounts");

            migrationBuilder.DropTable(
                name: "endLocations");

            migrationBuilder.DropTable(
                name: "startLocations");
        }
    }
}
