using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SternGarage.Models;
using SternGarage.Services.Contracts;

namespace SternGarage.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var reviews = await _reviewService.GetAllReviewsAsync();
            return View(reviews);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int? carId)
        {
            ViewBag.Cars = await _reviewService.GetCarSelectListAsync();

            var review = new Review { };
            if (carId.HasValue)
            {
                review.CarId = carId.Value;
            }

            return View(review);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Review review)
        {
            if (ModelState.IsValid)
            {
                await _reviewService.AddReviewAsync(review);
                TempData["SuccessMessage"] = "Отзивът е добавен!";
                return RedirectToAction("Details", "Cars", new { id = review.CarId });
            }

            ViewBag.Cars = await _reviewService.GetCarSelectListAsync();
            return View(review);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _reviewService.GetReviewByIdAsync(id.Value);

            if (review == null)
            {
                return NotFound();
            }

            ViewBag.Cars = await _reviewService.GetCarSelectListAsync();
            return View(review);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Review review)
        {
            if (id != review.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var success = await _reviewService.EditReviewAsync(id, review);

                    if (!success)
                    {
                        return NotFound();
                    }

                    TempData["SuccessMessage"] = "Отзивът е обновен!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _reviewService.ReviewExistsAsync(review.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction("Details", "Cars", new { id = review.CarId });
            }

            ViewBag.Cars = await _reviewService.GetCarSelectListAsync();
            return View(review);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var review = await _reviewService.GetReviewByIdAsync(id.Value);

            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var carId = await _reviewService.DeleteReviewAsync(id);
            TempData["SuccessMessage"] = "Отзивът е изтрит!";

            if (carId.HasValue)
            {
                return RedirectToAction("Details", "Cars", new { id = carId });
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
