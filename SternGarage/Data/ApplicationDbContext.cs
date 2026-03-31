using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SternGarage.Models;
using System;

namespace SternGarages.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<CarClass> CarClasses { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Favorite> Favorites { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Car>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.HasOne(c => c.Class)
                    .WithMany(cc => cc.Cars)
                    .HasForeignKey(c => c.ClassId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(c => c.Price)
                    .HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasKey(r => r.Id);

                entity.HasOne(r => r.Car)
                    .WithMany(c => c.Reviews)
                    .HasForeignKey(r => r.CarId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<CarClass>().HasData(
                new CarClass
                {
                    Id = 1,
                    Name = "C-Class",
                    Description = "Спортен седан от средния клас"
                },
                new CarClass
                {
                    Id = 2,
                    Name = "E-Class",
                    Description = "Бизнес седан, комфорт и технологии"
                },
                new CarClass
                {
                    Id = 3,
                    Name = "GLA",
                    Description = "Компактен SUV"
                },
                new CarClass
                {
                    Id = 4,
                    Name = "AMG GT",
                    Description = "Спортен автомобил, високопроизводителен"
                },
                new CarClass
                {
                    Id = 5,
                    Name = "EQS",
                    Description = "Електрически луксозен седан"
                },
                new CarClass
                {
                    Id = 6,
                    Name = "Maybach",
                    Description = "Ултра луксозни автомобили"
                }
            );

            modelBuilder.Entity<Car>().HasData(
                new Car
                {
                    Id = 1,
                    Model = "Mercedes-Benz C 300 4MATIC",
                    Year = 2023,
                    ClassId = 1,
                    Horsepower = 255,
                    EngineType = "2.0L Turbo Бензин + 48V Mild Hybrid",
                    Price = 48500,
                    Description = "C-Class комбинира спортен дизайн с технологии. 2.0L турбо с 48V mild hybrid дава 255 к.с. плюс допълнителни 20 к.с. при нужда. MBUX система, 9-степенна автоматична скоростна кутия.",
                    ImageUrl = "/images/cars/c300.jpg",
                },
                new Car
                {
                    Id = 2,
                    Model = "Mercedes-Benz E 350 4MATIC",
                    Year = 2025,
                    ClassId = 2,
                    Horsepower = 255,
                    EngineType = "2.0L Turbo Бензин + 48V Mild Hybrid",
                    Price = 64000,
                    Description = "Шестото поколение E-Class. MBUX Superscreen, Burmester 4D звук, AIRMATIC окачване. Двигателят е 2.0L турбо с mild hybrid - 255 к.с. и 9-степенна автоматична скоростна кутия.",
                    ImageUrl = "/images/cars/e350.jpg",
                },
                new Car
                {
                    Id = 3,
                    Model = "Mercedes-AMG GT 63 S E Performance",
                    Year = 2022,
                    ClassId = 4,
                    Horsepower = 831,
                    EngineType = "4.0L V8 Biturbo + Електромотор (PHEV)",
                    Price = 215000,
                    Description = "Един от най-мощните сериини AMG! 4-вратото купе има 4.0L V8 Biturbo който прави 630 к.с. + електромотор който добавя още 201 к.с. за обща мощност от 831 к.с. и над 1000 Nm. 0-100 за 2.8 сек.",
                    ImageUrl = "/images/cars/amg-gt63s.jpg",
                },
                new Car
                {
                    Id = 4,
                    Model = "Mercedes-Benz EQS 580 4MATIC",
                    Year = 2026,
                    ClassId = 5,
                    Horsepower = 536,
                    EngineType = "Dual Motor Electric (AWD)",
                    Price = 128000,
                    Description = "Флагманът на електрическата линия. Два мотора правейки 536 к.с. и 858 Nm със 118 kWh батерия и до 596 км пробег по WLTP. 56-инчов MBUX Hyperscreen идва стандартно. Бързо зареждане до 200 kW.",
                    ImageUrl = "/images/cars/eqs580.jpg",
                },
                new Car
                {
                    Id = 5,
                    Model = "Mercedes-Benz GLA 250 4MATIC",
                    Year = 2019,
                    ClassId = 3,
                    Horsepower = 208,
                    EngineType = "2.0L Turbo Бензин",
                    Price = 35950,
                    Description = "Компактен SUV с мощност от 208 к.с. от малкият 2.0L турбо с 350NM и 7-степенна автоматична скоростна кутия. 0-100 за 7.2 сек. Висока позиция на шофиране, добра маневреност. Разход около 13.5л/100км в града.",
                    ImageUrl = "/images/cars/gla250.jpg",
                },
                new Car
                {
                    Id = 6,
                    Model = "Mercedes-Maybach S 680 4MATIC",
                    Year = 2026,
                    ClassId = 6,
                    Horsepower = 621,
                    EngineType = "6.0L V12 Biturbo",
                    Price = 340000,
                    Description = "6.0L V12 Biturbo разполоага с 621 к.с. и 900NM. Ръчно изработен интериор, Executive задни седалки с масаж, Burmester High-End 4D, AIRMATIC и 4.5° задно управление. За 2026 има подгряване на коланите и Hot-Stone масаж.",
                    ImageUrl = "/images/cars/maybach-s680.jpg",
                }
            );

            modelBuilder.Entity<Review>().HasData(
                new Review
                {
                    Id = 1,
                    CarId = 1,
                    AuthorName = "Мария Иванова",
                    Title = "Добра кола за всеки ден",
                    Content = "Карам го от 6 месеца, MBUX-а е готин, седалките удобни. Разходът е около 7л извън града с mild hybrid-а. За тая цена е ок.",
                    Rating = 4,
                    CreatedAt = new DateTime(2023, 9, 12, 18, 30, 25, DateTimeKind.Utc)
                },
                new Review
                {
                    Id = 2,
                    CarId = 1,
                    AuthorName = "Стоян Георгиев",
                    Title = "Къде отиде духът на C-Class?",
                    Content = "Имах W204 C350 с 3.5 V6 и беше страхотна кола - малка, бърза, луксозна. Сега новият C-Class е само 4-цилиндров, няма го вече онзи звук и характер. Да, технологията вътре е на ниво, MBUX е супер, но колата вече е само за хора които искат лукс отвътре, не за ентусиасти. AMG C63 с 4-цилиндров мотор е просто тъжно. Мерцедес избра да направи C-Class tech кола вместо шофьорска кола.",
                    Rating = 2,
                    CreatedAt = new DateTime(2024, 2, 5, 8, 07, 13, DateTimeKind.Utc)
                },
                new Review
                {
                    Id = 3,
                    CarId = 3,
                    AuthorName = "Никола Колев",
                    Title = "Звяр",
                    Content = "V8 плюс електромотор, на писта е брутално. Тежка е малко но с тия коне не се усеща чак толкова.",
                    Rating = 5,
                    CreatedAt = new DateTime(2024, 1, 13, 13, 30, 55, DateTimeKind.Utc)
                },
                new Review
                {
                    Id = 4,
                    CarId = 4,
                    AuthorName = "Елена Димитрова",
                    Title = "Добра но зарядните станции са проблем",
                    Content = "Колата е тиха и бърза, Hyperscreen-a е готин. Ама в България зарядната инфраструктура е зле, на магистралата няма почти нищо.",
                    Rating = 3,
                    CreatedAt = new DateTime(2026, 2, 10, 15, 43, 37, DateTimeKind.Utc)
                }
            );
        }
    }
}
