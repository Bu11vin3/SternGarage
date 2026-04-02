using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SternGarage.Services.Contracts;

namespace SternGarage.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly IReviewService _reviewService;
        private readonly ICarService _carService;

        public AdminController(IReviewService reviewService, ICarService carService)
        {
            _reviewService = reviewService;
            _carService = carService;
        }

        public async Task<IActionResult> Index()
        {
            var reviews = await _reviewService.GetAllReviewsAsync();
            var totalCars = await _carService.GetTotalCarsCountAsync();

            ViewBag.TotalCars = totalCars;
            ViewBag.TotalReviews = await _reviewService.GetTotalReviewsCountAsync();

            return View(reviews);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteReview(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _reviewService.GetReviewForDeleteAsync(id.Value);

            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        [HttpPost, ActionName("DeleteReview")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteReviewConfirmed(int id)
        {
            await _reviewService.DeleteReviewAsync(id);
            TempData["SuccessMessage"] = "Отзивът беше изтрит успешно.";
            return RedirectToAction(nameof(Index));
        }
    }
}