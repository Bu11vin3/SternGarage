using MercedesBlog.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MercedesBlog.Services.Contracts
{
    public interface IReviewService
    {
        Task<IEnumerable<Review>> GetAllReviewsAsync();

        Task<IEnumerable<Review>> GetLatestReviewsAsync(int count);

        Task<Review?> GetReviewByIdAsync(int id);

        Task<Review?> GetReviewForDeleteAsync(int id);

        Task AddReviewAsync(Review review);

        Task<bool> EditReviewAsync(int id, Review review);

        Task<int?> DeleteReviewAsync(int id);

        Task<bool> ReviewExistsAsync(int id);

        Task<IEnumerable<SelectListItem>> GetCarSelectListAsync();

        Task<int> GetTotalReviewsCountAsync();
    }
}
