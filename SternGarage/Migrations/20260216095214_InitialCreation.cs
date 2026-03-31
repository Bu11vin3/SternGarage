using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SternGarage.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarClasses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarClasses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Model = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    ClassId = table.Column<int>(type: "int", nullable: false),
                    Horsepower = table.Column<int>(type: "int", nullable: false),
                    EngineType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cars_CarClasses_ClassId",
                        column: x => x.ClassId,
                        principalTable: "CarClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarId = table.Column<int>(type: "int", nullable: false),
                    AuthorName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CarClasses",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Спортен седан от средния клас", "C-Class" },
                    { 2, "Бизнес седан, комфорт и технологии", "E-Class" },
                    { 3, "Компактен SUV", "GLA" },
                    { 4, "Спортен автомобил, високопроизводителен", "AMG GT" },
                    { 5, "Електрически луксозен седан", "EQS" },
                    { 6, "Ултра луксозни автомобили", "Maybach" }
                });

            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "Id", "ClassId", "CreatedAt", "Description", "EngineType", "Horsepower", "ImageUrl", "Model", "Price", "UpdatedAt", "Year" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 2, 16, 9, 52, 14, 123, DateTimeKind.Utc).AddTicks(2839), "C-Class комбинира спортен дизайн с технологии. 2.0L турбо с 48V mild hybrid дава 255 к.с. плюс допълнителни 20 к.с. при нужда. MBUX система, 9-степенна автоматична скоростна кутия.", "2.0L Turbo Бензин + 48V Mild Hybrid", 255, "/images/cars/c300.jpg", "Mercedes-Benz C 300 4MATIC", 48500m, null, 2023 },
                    { 2, 2, new DateTime(2026, 2, 16, 9, 52, 14, 123, DateTimeKind.Utc).AddTicks(2845), "Шестото поколение E-Class. MBUX Superscreen, Burmester 4D звук, AIRMATIC окачване. Двигателят е 2.0L турбо с mild hybrid - 255 к.с. и 9-степенна автоматична скоростна кутия.", "2.0L Turbo Бензин + 48V Mild Hybrid", 255, "/images/cars/e350.jpg", "Mercedes-Benz E 350 4MATIC", 64000m, null, 2025 },
                    { 3, 4, new DateTime(2026, 2, 16, 9, 52, 14, 123, DateTimeKind.Utc).AddTicks(2847), "Един от най-мощните сериини AMG! 4-вратото купе има 4.0L V8 Biturbo който прави 630 к.с. + електромотор който добавя още 201 к.с. за обща мощност от 831 к.с. и над 1000 Nm. 0-100 за 2.8 сек. Има Drift Mode и AMG RIDE CONTROL+ окачване.", "4.0L V8 Biturbo + Електромотор (PHEV)", 831, "/images/cars/amg-gt63s.jpg", "Mercedes-AMG GT 63 S E Performance", 215000m, null, 2022 },
                    { 4, 5, new DateTime(2026, 2, 16, 9, 52, 14, 123, DateTimeKind.Utc).AddTicks(2848), "Флагманът на електрическата линия. Два мотора правейки 536 к.с. и 858 Nm със 118 kWh батерия и до 596 км пробег по WLTP. 56-инчов MBUX Hyperscreen идва стандартно. Бързо зареждане до 200 kW.", "Dual Motor Electric (AWD)", 536, "/images/cars/eqs580.jpg", "Mercedes-Benz EQS 580 4MATIC", 128000m, null, 2026 },
                    { 5, 3, new DateTime(2026, 2, 16, 9, 52, 14, 123, DateTimeKind.Utc).AddTicks(2849), "Компактен SUV с мощност от 208 к.с. от малкият 2.0L турбо с 350NM и 7-степенна автоматична скоростна кутия. 0-100 за 7.2 сек. Висока позиция на шофиране, добра маневреност. Разход около 13.5л/100км в града.", "2.0L Turbo Бензин", 208, "/images/cars/gla250.jpg", "Mercedes-Benz GLA 250 4MATIC", 35950m, null, 2019 },
                    { 6, 6, new DateTime(2026, 2, 16, 9, 52, 14, 123, DateTimeKind.Utc).AddTicks(2851), "6.0L V12 Biturbo разполоага с 621 к.с. и 900NM. Ръчно изработен интериор, Executive задни седалки с масаж, Burmester High-End 4D, AIRMATIC и 4.5° задно управление. За 2026 има подгряване на коланите и Hot-Stone масаж.", "6.0L V12 Biturbo", 621, "/images/cars/maybach-s680.jpg", "Mercedes-Maybach S 680 4MATIC", 340000m, null, 2026 }
                });

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "Id", "AuthorName", "CarId", "Content", "CreatedAt", "Rating", "Title" },
                values: new object[,]
                {
                    { 1, "Мария Иванова", 1, "Карам го от 6 месеца, MBUX-а е готин, седалките удобни. Разходът е около 7л извън града с mild hybrid-а. За тая цена е ок.", new DateTime(2023, 9, 12, 18, 30, 25, 0, DateTimeKind.Utc), 4, "Добра кола за всеки ден" },
                    { 2, "Стоян Георгиев", 1, "Имах W204 C350 с 3.5 V6 и беше страхотна кола - малка, бърза, луксозна. Сега новият C-Class е само 4-цилиндров, няма го вече онзи звук и характер. Да, технологията вътре е на ниво, MBUX е супер, но колата вече е само за хора които искат лукс отвътре, не за ентусиасти. AMG C63 с 4-цилиндров мотор е просто тъжно. Мерцедес избра да направи C-Class tech кола вместо шофьорска кола.", new DateTime(2024, 2, 5, 8, 7, 13, 0, DateTimeKind.Utc), 2, "Къде отиде духът на C-Class?" },
                    { 3, "Никола Колев", 3, "V8 плюс електромотор, на писта е брутално. Тежка е малко но с тия коне не се усеща чак толкова.", new DateTime(2024, 1, 13, 13, 30, 55, 0, DateTimeKind.Utc), 5, "Звяр" },
                    { 4, "Елена Димитрова", 4, "Колата е тиха и бърза, Hyperscreen-a е готин. Ама в България зарядната инфраструктура е зле, на магистралата няма почти нищо.", new DateTime(2026, 2, 10, 15, 43, 37, 0, DateTimeKind.Utc), 3, "Добра но зарядните станции са проблем" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cars_ClassId",
                table: "Cars",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_CarId",
                table: "Reviews",
                column: "CarId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "Cars");

            migrationBuilder.DropTable(
                name: "CarClasses");
        }
    }
}
