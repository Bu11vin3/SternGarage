using SternGarage.Models;

namespace SternGarage.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Car> LatestCars { get; set; } = new List<Car>();
        public IEnumerable<Review> LatestReviews { get; set; } = new List<Review>();
        public int TotalCars { get; set; }
        public int TotalReviews { get; set; }
        public IEnumerable<CarClass> Classes { get; set; } = new List<CarClass>();
    }
}
