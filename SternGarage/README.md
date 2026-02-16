# SternGarage - Mercedes-Benz Blog

Уеб приложение за любители на Mercedes-Benz в България. Проект за курс ASP.NET Fundamentals в SoftUni.

## Какво прави приложението

- Каталог с автомобили Mercedes-Benz (CRUD операции)
- Отзиви от собственици с оценки от 1 до 5
- Филтриране по клас и търсене по модел
- Сортиране по цена, година и мощност

## Технологии

- ASP.NET Core 8.0 (MVC)
- Entity Framework Core 8.0 + SQL Server LocalDB
- Bootstrap 5.3

## Как да се стартира приложението със Package Manager Console

1. `Добавя се име на машината в appsettings.json при Connection String-а`
2. `Add-Migration InitialDbCreate`
3. `Update-Database`