using MercedesBlog.ViewModels;
using MercedesBlog.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MercedesBlog.Services.Contracts
{
    public interface ICarService
    {
        Task<IEnumerable<Car>> GetAllCarsAsync(int? classId, string? searchTerm, string? sortBy);

        Task<CarDetailsViewModel?> GetCarDetailsByIdAsync(int id);

        Task<CarFormViewModel> GetCarForCreateAsync();

        Task AddCarAsync(CarFormViewModel model);

        Task<Car?> GetCarForDeleteAsync(int id);

        Task DeleteCarAsync(int id);

        Task<bool> CarExistsAsync(int id);

        Task<IEnumerable<SelectListItem>> GetClassSelectListAsync();

        Task<IEnumerable<CarClass>> GetAllClassesAsync();

        Task<int> GetTotalCarsCountAsync();
    }
}
