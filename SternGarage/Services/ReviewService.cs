using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SternGarage.Data;
using SternGarage.Models;
using SternGarage.Services.Contracts;

namespace SternGarage.Services
{
    public class ReviewService : IReviewService
    {
        private readonly ApplicationDbContext _context;

        public ReviewService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Review>> GetAllReviewsAsync()
        {
            return await _context.Reviews
                .Include(r => r.Car)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetLatestReviewsAsync(int count)
        {
            return await _context.Reviews
                .Include(r => r.Car)
                .OrderByDescending(r => r.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

        public async Task<Review?> GetReviewByIdAsync(int id)
        {
            return await _context.Reviews.FindAsync(id);
        }

        public async Task<Review?> GetReviewForDeleteAsync(int id)
        {
            return await _context.Reviews
                .Include(r => r.Car)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task AddReviewAsync(Review review)
        {
            review.CreatedAt = DateTime.UtcNow;
            _context.Add(review);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> EditReviewAsync(int id, Review review)
        {
            var existing = await _context.Reviews.FindAsync(id);
            if (existing == null)
            {
                return false;
            }

            existing.CarId = review.CarId;
            existing.AuthorName = review.AuthorName;
            existing.Title = review.Title;
            existing.Content = review.Content;
            existing.Rating = review.Rating;

            _context.Update(existing);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int?> DeleteReviewAsync(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return null;
            }

            int carId = review.CarId;
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return carId;
        }

        public async Task<bool> ReviewExistsAsync(int id)
        {
            return await _context.Reviews.AnyAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<SelectListItem>> GetCarSelectListAsync()
        {
            return await _context.Cars
                .OrderBy(c => c.Model)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Model
                })
                .ToListAsync();
        }

        public async Task<int> GetTotalReviewsCountAsync()
        {
            return await _context.Reviews.CountAsync();
        }
    }
}
