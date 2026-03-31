using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SternGarage.Migrations
{
    /// <inheritdoc />
    public partial class Workn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 16, 9, 54, 45, 78, DateTimeKind.Utc).AddTicks(3039));

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 16, 9, 54, 45, 78, DateTimeKind.Utc).AddTicks(3044));

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 16, 9, 54, 45, 78, DateTimeKind.Utc).AddTicks(3047));

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 16, 9, 54, 45, 78, DateTimeKind.Utc).AddTicks(3050));

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 16, 9, 54, 45, 78, DateTimeKind.Utc).AddTicks(3055));

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 16, 9, 54, 45, 78, DateTimeKind.Utc).AddTicks(3056));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 16, 9, 52, 14, 123, DateTimeKind.Utc).AddTicks(2839));

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 16, 9, 52, 14, 123, DateTimeKind.Utc).AddTicks(2845));

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 16, 9, 52, 14, 123, DateTimeKind.Utc).AddTicks(2847));

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 16, 9, 52, 14, 123, DateTimeKind.Utc).AddTicks(2848));

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 16, 9, 52, 14, 123, DateTimeKind.Utc).AddTicks(2849));

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 16, 9, 52, 14, 123, DateTimeKind.Utc).AddTicks(2851));
        }
    }
}
