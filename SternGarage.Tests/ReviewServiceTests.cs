using Microsoft.EntityFrameworkCore;
using SternGarage.Models;
using SternGarage.Services;
using SternGarages.Data;

namespace SternGarage.Tests
{
    public class ReviewServiceTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly ReviewService _reviewService;

        public ReviewServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _context.Database.EnsureCreated();
            _reviewService = new ReviewService(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        // ==================== GetAllReviewsAsync ====================

        [Fact]
        public async Task GetAllReviewsAsync_ReturnsAllSeededReviews()
        {
            var result = await _reviewService.GetAllReviewsAsync();

            Assert.Equal(4, result.Count());
        }

        [Fact]
        public async Task GetAllReviewsAsync_OrderedByCreatedAtDesc()
        {
            var result = (await _reviewService.GetAllReviewsAsync()).ToList();

            for (int i = 1; i < result.Count; i++)
            {
                Assert.True(result[i].CreatedAt <= result[i - 1].CreatedAt);
            }
        }

        [Fact]
        public async Task GetAllReviewsAsync_IncludesCar()
        {
            var result = (await _reviewService.GetAllReviewsAsync()).ToList();

            Assert.All(result, r => Assert.NotNull(r.Car));
        }

        // ==================== GetPaginatedReviewsAsync ====================

        [Fact]
        public async Task GetPaginatedReviewsAsync_ReturnsCorrectPageSize()
        {
            var result = await _reviewService.GetPaginatedReviewsAsync(1, 2);

            Assert.Equal(2, result.Count);
            Assert.Equal(2, result.TotalPages);
        }

        [Fact]
        public async Task GetPaginatedReviewsAsync_SecondPage_ReturnsRemaining()
        {
            var result = await _reviewService.GetPaginatedReviewsAsync(2, 2);

            Assert.Equal(2, result.Count);
            Assert.True(result.HasPreviousPage);
            Assert.False(result.HasNextPage);
        }

        [Fact]
        public async Task GetPaginatedReviewsAsync_FirstPage_HasNoPreivousPage()
        {
            var result = await _reviewService.GetPaginatedReviewsAsync(1, 2);

            Assert.False(result.HasPreviousPage);
            Assert.True(result.HasNextPage);
        }

        [Fact]
        public async Task GetPaginatedReviewsAsync_OrderedByCreatedAtDesc()
        {
            var result = await _reviewService.GetPaginatedReviewsAsync(1, 10);

            for (int i = 1; i < result.Count; i++)
            {
                Assert.True(result[i].CreatedAt <= result[i - 1].CreatedAt);
            }
        }

        // ==================== GetLatestReviewsAsync ====================

        [Fact]
        public async Task GetLatestReviewsAsync_ReturnsRequestedCount()
        {
            var result = await _reviewService.GetLatestReviewsAsync(2);

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetLatestReviewsAsync_ReturnsNewestFirst()
        {
            var result = (await _reviewService.GetLatestReviewsAsync(4)).ToList();

            for (int i = 1; i < result.Count; i++)
            {
                Assert.True(result[i].CreatedAt <= result[i - 1].CreatedAt);
            }
        }

        [Fact]
        public async Task GetLatestReviewsAsync_CountExceedsTotal_ReturnsAll()
        {
            var result = await _reviewService.GetLatestReviewsAsync(100);

            Assert.Equal(4, result.Count());
        }

        // ==================== GetReviewByIdAsync ====================

        [Fact]
        public async Task GetReviewByIdAsync_ExistingReview_ReturnsReview()
        {
            var result = await _reviewService.GetReviewByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal("Мария Иванова", result.AuthorName);
            Assert.Equal(4, result.Rating);
        }

        [Fact]
        public async Task GetReviewByIdAsync_NonExistent_ReturnsNull()
        {
            var result = await _reviewService.GetReviewByIdAsync(999);

            Assert.Null(result);
        }

        // ==================== GetReviewForDeleteAsync ====================

        [Fact]
        public async Task GetReviewForDeleteAsync_ExistingReview_ReturnsWithCar()
        {
            var result = await _reviewService.GetReviewForDeleteAsync(1);

            Assert.NotNull(result);
            Assert.NotNull(result.Car);
            Assert.Equal(1, result.CarId);
        }

        [Fact]
        public async Task GetReviewForDeleteAsync_NonExistent_ReturnsNull()
        {
            var result = await _reviewService.GetReviewForDeleteAsync(999);

            Assert.Null(result);
        }

        // ==================== AddReviewAsync ====================

        [Fact]
        public async Task AddReviewAsync_AddsReviewToDatabase()
        {
            var review = new Review
            {
                CarId = 2,
                AuthorName = "Тест Автор",
                Title = "Тестово ревю заглавие",
                Content = "Тестово съдържание на ревю за кола",
                Rating = 5
            };

            await _reviewService.AddReviewAsync(review);

            Assert.Equal(5, await _context.Reviews.CountAsync());
            var added = await _context.Reviews.FirstAsync(r => r.AuthorName == "Тест Автор");
            Assert.Equal(5, added.Rating);
            Assert.Equal(2, added.CarId);
        }

        [Fact]
        public async Task AddReviewAsync_SetsCreatedAt()
        {
            var review = new Review
            {
                CarId = 1,
                AuthorName = "Нов Автор",
                Title = "Ново заглавие за тест",
                Content = "Ново съдържание за тестово ревю",
                Rating = 3
            };

            var before = DateTime.UtcNow;
            await _reviewService.AddReviewAsync(review);
            var after = DateTime.UtcNow;

            var added = await _context.Reviews.FirstAsync(r => r.AuthorName == "Нов Автор");
            Assert.InRange(added.CreatedAt, before, after);
        }

        // ==================== EditReviewAsync ====================

        [Fact]
        public async Task EditReviewAsync_ExistingReview_UpdatesAndReturnsTrue()
        {
            var updated = new Review
            {
                CarId = 1,
                AuthorName = "Обновен Автор",
                Title = "Обновено заглавие за тест",
                Content = "Обновено съдържание на ревю",
                Rating = 1
            };

            var result = await _reviewService.EditReviewAsync(1, updated);

            Assert.True(result);
            var review = await _context.Reviews.FindAsync(1);
            Assert.Equal("Обновен Автор", review!.AuthorName);
            Assert.Equal(1, review.Rating);
        }

        [Fact]
        public async Task EditReviewAsync_NonExistent_ReturnsFalse()
        {
            var updated = new Review
            {
                CarId = 1,
                AuthorName = "Ghost",
                Title = "Ghost Title For Test",
                Content = "Ghost content for test review",
                Rating = 3
            };

            var result = await _reviewService.EditReviewAsync(999, updated);

            Assert.False(result);
        }

        // ==================== DeleteReviewAsync ====================

        [Fact]
        public async Task DeleteReviewAsync_ExistingReview_RemovesAndReturnsCarId()
        {
            var result = await _reviewService.DeleteReviewAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.Value);
            Assert.Equal(3, await _context.Reviews.CountAsync());
        }

        [Fact]
        public async Task DeleteReviewAsync_NonExistent_ReturnsNull()
        {
            var result = await _reviewService.DeleteReviewAsync(999);

            Assert.Null(result);
            Assert.Equal(4, await _context.Reviews.CountAsync());
        }

        // ==================== ReviewExistsAsync ====================

        [Fact]
        public async Task ReviewExistsAsync_ExistingReview_ReturnsTrue()
        {
            var result = await _reviewService.ReviewExistsAsync(1);

            Assert.True(result);
        }

        [Fact]
        public async Task ReviewExistsAsync_NonExistent_ReturnsFalse()
        {
            var result = await _reviewService.ReviewExistsAsync(999);

            Assert.False(result);
        }

        // ==================== GetCarSelectListAsync ====================

        [Fact]
        public async Task GetCarSelectListAsync_ReturnsAllCarsAsSelectItems()
        {
            var result = (await _reviewService.GetCarSelectListAsync()).ToList();

            Assert.Equal(6, result.Count);
            Assert.All(result, item =>
            {
                Assert.NotNull(item.Value);
                Assert.NotNull(item.Text);
            });
        }

        [Fact]
        public async Task GetCarSelectListAsync_OrderedByModel()
        {
            var result = (await _reviewService.GetCarSelectListAsync()).ToList();

            for (int i = 1; i < result.Count; i++)
            {
                Assert.True(string.Compare(result[i].Text, result[i - 1].Text, StringComparison.Ordinal) >= 0);
            }
        }

        // ==================== GetTotalReviewsCountAsync ====================

        [Fact]
        public async Task GetTotalReviewsCountAsync_ReturnsCorrectCount()
        {
            var result = await _reviewService.GetTotalReviewsCountAsync();

            Assert.Equal(4, result);
        }

        [Fact]
        public async Task GetTotalReviewsCountAsync_AfterAdding_ReturnsUpdatedCount()
        {
            _context.Reviews.Add(new Review
            {
                CarId = 1,
                AuthorName = "Допълнителен Автор",
                Title = "Допълнително заглавие тест",
                Content = "Допълнително съдържание за тест ревю",
                Rating = 4
            });
            await _context.SaveChangesAsync();

            var result = await _reviewService.GetTotalReviewsCountAsync();

            Assert.Equal(5, result);
        }
    }
}
