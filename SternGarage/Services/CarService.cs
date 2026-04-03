using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SternGarage.Models;
using SternGarage.ViewModels;
using SternGarage.Services.Contracts;
using SternGarages.Data;

namespace SternGarage.Services
{
    public class CarService : ICarService
    {
        private readonly ApplicationDbContext _context;

        public CarService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Car>> GetAllCarsAsync(int? classId, string? searchTerm, string? sortBy)
        {
            var query = _context.Cars
                .Include(c => c.Class)
                .Include(c => c.Reviews)
                .AsQueryable();

            if (classId.HasValue)
            {
                query = query.Where(c => c.ClassId == classId.Value);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(c => c.Model.Contains(searchTerm) || 
                                        (c.Description != null && c.Description.Contains(searchTerm)));
            }

            if (sortBy == "price_asc")
            {
                query = query.OrderBy(c => c.Price);
            }
            else if (sortBy == "price_desc")
            {
                query = query.OrderByDescending(c => c.Price);
            }
            else if (sortBy == "year_desc")
            {
                query = query.OrderByDescending(c => c.Year);
            }
            else if (sortBy == "year_asc")
            {
                query = query.OrderBy(c => c.Year);
            }
            else if (sortBy == "power_desc")
            {
                query = query.OrderByDescending(c => c.Horsepower);
            }
            else
            {
                query = query.OrderByDescending(c => c.Id);
            }

            return await query.ToListAsync();
        }

        public async Task<CarDetailsViewModel?> GetCarDetailsByIdAsync(int id)
        {
            var car = await _context.Cars
                .Include(c => c.Class)
                .Include(c => c.Reviews)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (car == null)
            {
                return null;
            }

            return new CarDetailsViewModel
            {
                Id = car.Id,
                Model = car.Model,
                Year = car.Year,
                ClassName = car.Class?.Name ?? "Неизвестен",
                Horsepower = car.Horsepower,
                EngineType = car.EngineType,
                Price = car.Price,
                Description = car.Description,
                ImageUrl = car.ImageUrl,
                AverageRating = car.Reviews.Any() ? car.Reviews.Average(r => r.Rating) : 0,
                ReviewCount = car.Reviews.Count,
                Reviews = car.Reviews.OrderByDescending(r => r.CreatedAt)
            };
        }

        public async Task<CarFormViewModel> GetCarForCreateAsync()
        {
            return new CarFormViewModel
            {
                Year = DateTime.Now.Year,
                Classes = await GetClassSelectListAsync()
            };
        }

        public async Task AddCarAsync(CarFormViewModel model)
        {
            var car = new Car
            {
                Model = model.Model,
                Year = model.Year,
                ClassId = model.ClassId,
                Horsepower = model.Horsepower,
                EngineType = model.EngineType,
                Price = model.Price,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
            };

            _context.Add(car);
            await _context.SaveChangesAsync();
        }

        public async Task<CarFormViewModel?> GetCarForEditAsync(int id)
        {
            var car = await _context.Cars.FindAsync(id);

            if (car == null)
            {
                return null;
            }

            return new CarFormViewModel
            {
                Id = car.Id,
                Model = car.Model,
                Year = car.Year,
                ClassId = car.ClassId,
                Horsepower = car.Horsepower,
                EngineType = car.EngineType,
                Price = car.Price,
                Description = car.Description,
                ImageUrl = car.ImageUrl,
                Classes = await GetClassSelectListAsync()
            };
        }

        public async Task UpdateCarAsync(CarFormViewModel model)
        {
            var car = await _context.Cars.FindAsync(model.Id);

            if (car != null)
            {
                car.Model = model.Model;
                car.Year = model.Year;
                car.ClassId = model.ClassId;
                car.Horsepower = model.Horsepower;
                car.EngineType = model.EngineType;
                car.Price = model.Price;
                car.Description = model.Description;
                car.ImageUrl = model.ImageUrl;
                car.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
            }
        }

        public async Task<Car?> GetCarForDeleteAsync(int id)
        {
            return await _context.Cars
                .Include(c => c.Class)
                .Include(c => c.Reviews)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task DeleteCarAsync(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car != null)
            {
                _context.Cars.Remove(car);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> CarExistsAsync(int id)
        {
            return await _context.Cars.AnyAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<SelectListItem>> GetClassSelectListAsync()
        {
            return await _context.CarClasses
                .OrderBy(c => c.Name)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<CarClass>> GetAllClassesAsync()
        {
            return await _context.CarClasses.ToListAsync();
        }

        public async Task<int> GetTotalCarsCountAsync()
        {
            return await _context.Cars.CountAsync();
        }
    }
}
