# SternGarage - Mercedes-Benz Blog

Уеб приложение за любители на Mercedes-Benz в България. Проект за курс ASP.NET Advanced в SoftUni.

Приложението предоставя каталог с автомобили Mercedes-Benz, система за отзиви с оценки и административен панел за управление на съдържанието.

## Функционалности

### Автомобили
- Преглед на каталог с пагинация (6 коли на страница)
- Детайлна страница за всеки автомобил с ревюта и средна оценка
- Филтриране по клас (C-Class, E-Class, GLA, AMG GT, EQS, Maybach)
- Търсене по модел и описание
- Сортиране по цена, година и мощност
- CRUD операции (Create, Read, Update, Delete) за администратори

### Отзиви
- Система за ревюта с оценка от 1 до 5 звезди
- Пагинация на ревюта (5 на страница)
- Създаване, редактиране и изтриване на ревюта

### Потребители и роли
- Регистрация и вход с ASP.NET Identity
- Две роли: **Administrator** и **User**
- Администраторски панел за управление на коли и ревюта
- Ограничен достъп до CRUD операции само за администратори

### Други
- Персонализирани 404 и 500 грешки страници
- Респонсив дизайн с Bootstrap 5.3.2
- Начална страница с последни коли, ревюта и статистики


## Архитектура

Проектът следва **MVC + Service Layer** архитектура:

- **Models** — Entity модели с валидации (Car, CarClass, Review, Favorite, ApplicationUser)
- **Views** — Razor изгледи с partial views и секции, Bootstrap 5.3.2 responsive дизайн
- **Controllers** — CarsController, ReviewsController, HomeController + Area за Identity
- **Services** — CarService и ReviewService капсулират бизнес логиката и работят с базата чрез EF Core
- **Contracts** — интерфейси (ICarService, IReviewService) за Dependency Injection
- **ViewModels** — CarFormViewModel, CarDetailsViewModel, HomeViewModel, PaginatedList\<T\>
- **Data** — ApplicationDbContext с конфигурации, seed данни и миграции

## Технологии

| Технология | Версия | Предназначение |
|---|---|---|
| ASP.NET Core | 8.0 | MVC уеб framework |
| Entity Framework Core | 8.0 | ORM за достъп до база данни |
| SQL Server | 2022 | Релационна база данни |
| ASP.NET Identity | 8.0 | Автентикация и авторизация |
| Bootstrap | 5.3.2 | Респонсив UI |
| xUnit | 2.x | Unit тестване |
| EF Core InMemory | 8.0 | Тестова база данни |

## База данни

### Модели
- **Car** — модел, година, клас, мощност, тип двигател, цена, описание, снимка
- **CarClass** — 6 класа Mercedes-Benz (C-Class, E-Class, GLA, AMG GT, EQS, Maybach)
- **Review** — автор, заглавие, съдържание, оценка (1-5), връзка към кола
- **ApplicationUser** — разширен IdentityUser с FullName и RegisteredAt
- **Favorite** — любими коли на потребител

### Seed данни
- 6 класа автомобили с описания
- 6 автомобила Mercedes-Benz (от GLA 250 до Maybach S 680)
- 4 ревюта от различни потребители
- Роли: Administrator, User
- Тестов потребител: `test@test.com` / `Test123!`
- Администратор: `OlaK@MB.com` / `Merc0n7op!`

## Валидации и сигурност

- **Model validation** — Required, StringLength, Range за всички полета
- **AntiForgeryToken** — CSRF защита на всички форми
- **XSS защита** — автоматично Razor encoding
- **Authorize атрибут** — роля-базиран достъп до админ функции
- **Parameter tampering check** — проверка при Edit за несъответствие на ID

## Тестове

64 unit теста покриващи 100% от методите на service слоя:

- **CarService** — 38 теста за 13 метода (CRUD, пагинация, филтриране, сортиране, търсене)
- **ReviewService** — 26 теста за 11 метода (CRUD, пагинация, рейтинги)

Изпълнение:
```bash
dotnet test SternGarage.Tests/SternGarage.Tests.csproj --verbosity normal
```

## Дизайнерски решения

- Service Layer шаблон — контролерите не достъпват базата директно, а през сервизи с интерфейси за лесно тестване и loose coupling
- Generic `PaginatedList<T>` — преизползваем компонент за пагинация на Cars и Reviews
- Seed данни в `OnModelCreating` — базата се попълва автоматично с 6 класа, 6 коли и 4 ревюта
- `UseStatusCodePagesWithReExecute` middleware — пренасочва HTTP грешки към custom views без промяна на URL-а
- InMemory database за тестове — бързи и изолирани unit тестове без зависимост от SQL Server

## Как да се стартира

1. Отворете `SternGarage.slnx` във Visual Studio
2. Променете connection string-а в `appsettings.json` (`DefaultConnection`) с името на вашата машина
3. В Package Manager Console:
   ```
   Add-Migration InitialDbCreate
   Update-Database
   ```
4. Стартирайте с F5
