using Microsoft.EntityFrameworkCore;
using SternGarage.Models;
using SternGarage.Services;
using SternGarage.ViewModels;
using SternGarages.Data;

namespace SternGarage.Tests
{
    public class CarServiceTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly CarService _carService;

        public CarServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _context.Database.EnsureCreated();
            _carService = new CarService(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        // ==================== GetAllCarsAsync ====================

        [Fact]
        public async Task GetAllCarsAsync_ReturnsAllSeededCars()
        {
            var result = await _carService.GetAllCarsAsync(null, null, null);

            Assert.Equal(6, result.Count());
        }

        [Fact]
        public async Task GetAllCarsAsync_FiltersByClassId()
        {
            var result = await _carService.GetAllCarsAsync(1, null, null);

            Assert.All(result, c => Assert.Equal(1, c.ClassId));
        }

        [Fact]
        public async Task GetAllCarsAsync_FiltersBySearchTerm()
        {
            var result = await _carService.GetAllCarsAsync(null, "AMG", null);

            Assert.Single(result);
            Assert.Contains("AMG", result.First().Model);
        }

        [Fact]
        public async Task GetAllCarsAsync_SortsByPriceAsc()
        {
            var result = (await _carService.GetAllCarsAsync(null, null, "price_asc")).ToList();

            for (int i = 1; i < result.Count; i++)
            {
                Assert.True(result[i].Price >= result[i - 1].Price);
            }
        }

        [Fact]
        public async Task GetAllCarsAsync_SortsByPriceDesc()
        {
            var result = (await _carService.GetAllCarsAsync(null, null, "price_desc")).ToList();

            for (int i = 1; i < result.Count; i++)
            {
                Assert.True(result[i].Price <= result[i - 1].Price);
            }
        }

        [Fact]
        public async Task GetAllCarsAsync_SortsByYearDesc()
        {
            var result = (await _carService.GetAllCarsAsync(null, null, "year_desc")).ToList();

            for (int i = 1; i < result.Count; i++)
            {
                Assert.True(result[i].Year <= result[i - 1].Year);
            }
        }

        [Fact]
        public async Task GetAllCarsAsync_SortsByYearAsc()
        {
            var result = (await _carService.GetAllCarsAsync(null, null, "year_asc")).ToList();

            for (int i = 1; i < result.Count; i++)
            {
                Assert.True(result[i].Year >= result[i - 1].Year);
            }
        }

        [Fact]
        public async Task GetAllCarsAsync_SortsByPowerDesc()
        {
            var result = (await _carService.GetAllCarsAsync(null, null, "power_desc")).ToList();

            for (int i = 1; i < result.Count; i++)
            {
                Assert.True(result[i].Horsepower <= result[i - 1].Horsepower);
            }
        }

        [Fact]
        public async Task GetAllCarsAsync_DefaultSortsByIdDesc()
        {
            var result = (await _carService.GetAllCarsAsync(null, null, null)).ToList();

            for (int i = 1; i < result.Count; i++)
            {
                Assert.True(result[i].Id < result[i - 1].Id);
            }
        }

        [Fact]
        public async Task GetAllCarsAsync_SearchTermNoMatch_ReturnsEmpty()
        {
            var result = await _carService.GetAllCarsAsync(null, "NonExistentModel12345", null);

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllCarsAsync_CombinesClassFilterAndSearch()
        {
            var result = await _carService.GetAllCarsAsync(1, "C 300", null);

            Assert.Single(result);
            Assert.Equal("Mercedes-Benz C 300 4MATIC", result.First().Model);
        }

        // ==================== GetPaginatedCarsAsync ====================

        [Fact]
        public async Task GetPaginatedCarsAsync_ReturnsCorrectPageSize()
        {
            var result = await _carService.GetPaginatedCarsAsync(null, null, null, 1, 3);

            Assert.Equal(3, result.Count);
            Assert.Equal(2, result.TotalPages);
        }

        [Fact]
        public async Task GetPaginatedCarsAsync_SecondPage_ReturnsRemainingItems()
        {
            var result = await _carService.GetPaginatedCarsAsync(null, null, null, 2, 4);

            Assert.Equal(2, result.Count);
            Assert.Equal(2, result.TotalPages);
        }

        [Fact]
        public async Task GetPaginatedCarsAsync_HasPreviousAndNextPage()
        {
            var page1 = await _carService.GetPaginatedCarsAsync(null, null, null, 1, 2);
            var page2 = await _carService.GetPaginatedCarsAsync(null, null, null, 2, 2);

            Assert.False(page1.HasPreviousPage);
            Assert.True(page1.HasNextPage);
            Assert.True(page2.HasPreviousPage);
            Assert.True(page2.HasNextPage);
        }

        [Fact]
        public async Task GetPaginatedCarsAsync_LastPage_NoNextPage()
        {
            var result = await _carService.GetPaginatedCarsAsync(null, null, null, 3, 2);

            Assert.False(result.HasNextPage);
            Assert.True(result.HasPreviousPage);
        }

        [Fact]
        public async Task GetPaginatedCarsAsync_WithFilters_PaginatesFilteredResults()
        {
            var result = await _carService.GetPaginatedCarsAsync(1, null, null, 1, 10);

            Assert.All(result, c => Assert.Equal(1, c.ClassId));
        }

        // ==================== GetCarDetailsByIdAsync ====================

        [Fact]
        public async Task GetCarDetailsByIdAsync_ExistingCar_ReturnsDetails()
        {
            var result = await _carService.GetCarDetailsByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal("Mercedes-Benz C 300 4MATIC", result.Model);
            Assert.Equal(2023, result.Year);
            Assert.Equal("C-Class", result.ClassName);
            Assert.Equal(255, result.Horsepower);
            Assert.Equal(48500m, result.Price);
        }

        [Fact]
        public async Task GetCarDetailsByIdAsync_CalculatesAverageRating()
        {
            var result = await _carService.GetCarDetailsByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(2, result.ReviewCount);
            Assert.Equal(3.0, result.AverageRating);
        }

        [Fact]
        public async Task GetCarDetailsByIdAsync_CarWithNoReviews_ZeroRating()
        {
            var result = await _carService.GetCarDetailsByIdAsync(2);

            Assert.NotNull(result);
            Assert.Equal(0, result.ReviewCount);
            Assert.Equal(0, result.AverageRating);
        }

        [Fact]
        public async Task GetCarDetailsByIdAsync_NonExistentCar_ReturnsNull()
        {
            var result = await _carService.GetCarDetailsByIdAsync(999);

            Assert.Null(result);
        }

        // ==================== GetCarForCreateAsync ====================

        [Fact]
        public async Task GetCarForCreateAsync_ReturnsFormWithClasses()
        {
            var result = await _carService.GetCarForCreateAsync();

            Assert.NotNull(result);
            Assert.Equal(DateTime.Now.Year, result.Year);
            Assert.NotNull(result.Classes);
            Assert.Equal(6, result.Classes.Count());
        }

        // ==================== AddCarAsync ====================

        [Fact]
        public async Task AddCarAsync_AddsCarToDatabase()
        {
            var model = new CarFormViewModel
            {
                Model = "Test Car",
                Year = 2024,
                ClassId = 1,
                Horsepower = 300,
                EngineType = "3.0L V6",
                Price = 55000m,
                Description = "Test description",
                ImageUrl = "https://example.com/img.jpg"
            };

            await _carService.AddCarAsync(model);

            Assert.Equal(7, await _context.Cars.CountAsync());
            var added = await _context.Cars.FirstAsync(c => c.Model == "Test Car");
            Assert.Equal(2024, added.Year);
            Assert.Equal(300, added.Horsepower);
            Assert.Equal(55000m, added.Price);
        }

        [Fact]
        public async Task AddCarAsync_WithoutOptionalFields_Works()
        {
            var model = new CarFormViewModel
            {
                Model = "Minimal Car",
                Year = 2023,
                ClassId = 2,
                Horsepower = 150,
                EngineType = "2.0L Turbo",
                Price = 30000m
            };

            await _carService.AddCarAsync(model);

            var added = await _context.Cars.FirstAsync(c => c.Model == "Minimal Car");
            Assert.Null(added.Description);
            Assert.Null(added.ImageUrl);
        }

        // ==================== GetCarForEditAsync ====================

        [Fact]
        public async Task GetCarForEditAsync_ExistingCar_ReturnsFormWithData()
        {
            var result = await _carService.GetCarForEditAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Mercedes-Benz C 300 4MATIC", result.Model);
            Assert.Equal(2023, result.Year);
            Assert.Equal(1, result.ClassId);
            Assert.NotNull(result.Classes);
            Assert.Equal(6, result.Classes.Count());
        }

        [Fact]
        public async Task GetCarForEditAsync_NonExistentCar_ReturnsNull()
        {
            var result = await _carService.GetCarForEditAsync(999);

            Assert.Null(result);
        }

        // ==================== UpdateCarAsync ====================

        [Fact]
        public async Task UpdateCarAsync_UpdatesCarProperties()
        {
            var model = new CarFormViewModel
            {
                Id = 1,
                Model = "Updated C 300",
                Year = 2025,
                ClassId = 1,
                Horsepower = 280,
                EngineType = "2.0L Turbo Updated",
                Price = 52000m,
                Description = "Updated description",
                ImageUrl = "https://example.com/updated.jpg"
            };

            await _carService.UpdateCarAsync(model);

            var updated = await _context.Cars.FindAsync(1);
            Assert.NotNull(updated);
            Assert.Equal("Updated C 300", updated.Model);
            Assert.Equal(2025, updated.Year);
            Assert.Equal(280, updated.Horsepower);
            Assert.Equal(52000m, updated.Price);
            Assert.Equal("Updated description", updated.Description);
            Assert.NotNull(updated.UpdatedAt);
        }

        [Fact]
        public async Task UpdateCarAsync_NonExistentCar_DoesNothing()
        {
            var model = new CarFormViewModel
            {
                Id = 999,
                Model = "Ghost Car",
                Year = 2024,
                ClassId = 1,
                Horsepower = 100,
                EngineType = "Test",
                Price = 10000m
            };

            await _carService.UpdateCarAsync(model);

            Assert.Equal(6, await _context.Cars.CountAsync());
        }

        // ==================== GetCarForDeleteAsync ====================

        [Fact]
        public async Task GetCarForDeleteAsync_ExistingCar_ReturnsCarWithIncludes()
        {
            var result = await _carService.GetCarForDeleteAsync(1);

            Assert.NotNull(result);
            Assert.Equal("Mercedes-Benz C 300 4MATIC", result.Model);
            Assert.NotNull(result.Class);
            Assert.NotNull(result.Reviews);
        }

        [Fact]
        public async Task GetCarForDeleteAsync_NonExistentCar_ReturnsNull()
        {
            var result = await _carService.GetCarForDeleteAsync(999);

            Assert.Null(result);
        }

        // ==================== DeleteCarAsync ====================

        [Fact]
        public async Task DeleteCarAsync_ExistingCar_RemovesFromDatabase()
        {
            await _carService.DeleteCarAsync(5);

            Assert.Equal(5, await _context.Cars.CountAsync());
            Assert.Null(await _context.Cars.FindAsync(5));
        }

        [Fact]
        public async Task DeleteCarAsync_NonExistentCar_DoesNothing()
        {
            await _carService.DeleteCarAsync(999);

            Assert.Equal(6, await _context.Cars.CountAsync());
        }

        // ==================== CarExistsAsync ====================

        [Fact]
        public async Task CarExistsAsync_ExistingCar_ReturnsTrue()
        {
            var result = await _carService.CarExistsAsync(1);

            Assert.True(result);
        }

        [Fact]
        public async Task CarExistsAsync_NonExistentCar_ReturnsFalse()
        {
            var result = await _carService.CarExistsAsync(999);

            Assert.False(result);
        }

        // ==================== GetClassSelectListAsync ====================

        [Fact]
        public async Task GetClassSelectListAsync_ReturnsAllClassesAsSelectItems()
        {
            var result = (await _carService.GetClassSelectListAsync()).ToList();

            Assert.Equal(6, result.Count);
            Assert.All(result, item =>
            {
                Assert.NotNull(item.Value);
                Assert.NotNull(item.Text);
            });
        }

        [Fact]
        public async Task GetClassSelectListAsync_OrderedByName()
        {
            var result = (await _carService.GetClassSelectListAsync()).ToList();

            for (int i = 1; i < result.Count; i++)
            {
                Assert.True(string.Compare(result[i].Text, result[i - 1].Text, StringComparison.Ordinal) >= 0);
            }
        }

        // ==================== GetAllClassesAsync ====================

        [Fact]
        public async Task GetAllClassesAsync_ReturnsAllClasses()
        {
            var result = await _carService.GetAllClassesAsync();

            Assert.Equal(6, result.Count());
        }

        // ==================== GetTotalCarsCountAsync ====================

        [Fact]
        public async Task GetTotalCarsCountAsync_ReturnsCorrectCount()
        {
            var result = await _carService.GetTotalCarsCountAsync();

            Assert.Equal(6, result);
        }

        [Fact]
        public async Task GetTotalCarsCountAsync_AfterAdding_ReturnsUpdatedCount()
        {
            _context.Cars.Add(new Car
            {
                Model = "Extra Car",
                Year = 2024,
                ClassId = 1,
                Horsepower = 200,
                EngineType = "Test",
                Price = 40000m
            });
            await _context.SaveChangesAsync();

            var result = await _carService.GetTotalCarsCountAsync();

            Assert.Equal(7, result);
        }
    }
}
