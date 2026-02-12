using SternGarage.Models;
using SternGarage.Services.Contracts;
using SternGarage.ViewModels;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using SternGarage.Models;
using System.Diagnostics;

namespace SternGarage.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICarService _carService;
        private readonly IReviewService _reviewService;

        public HomeController(ICarService carService, IReviewService reviewService)
        {
            _carService = carService;
            _reviewService = reviewService;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new HomeViewModel
            {
                LatestCars = await _carService.GetAllCarsAsync(null, null, null),
                LatestReviews = await _reviewService.GetLatestReviewsAsync(3),
                TotalCars = await _carService.GetTotalCarsCountAsync(),
                TotalReviews = await _reviewService.GetTotalReviewsCountAsync(),
                Classes = await _carService.GetAllClassesAsync()
            };

            viewModel.LatestCars = viewModel.LatestCars.Take(6);

            return View(viewModel);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
