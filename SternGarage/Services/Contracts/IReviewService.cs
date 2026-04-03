using SternGarage.Models;
using SternGarage.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SternGarage.Services.Contracts
{
    public interface IReviewService
    {
        Task<IEnumerable<Review>> GetAllReviewsAsync();

        Task<PaginatedList<Review>> GetPaginatedReviewsAsync(int pageIndex, int pageSize);

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
