using SternGarage.Models;

namespace SternGarage.ViewModels
{
    public class CarDetailsViewModel
    {
        public int Id { get; set; }
        public string Model { get; set; } = null!;
        public int Year { get; set; }
        public string ClassName { get; set; } = null!;
        public int Horsepower { get; set; }
        public string EngineType { get; set; } = null!;
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }
        public IEnumerable<Review> Reviews { get; set; } = new List<Review>();
    }
}
