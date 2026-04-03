using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SternGarage.Models;
using SternGarage.ViewModels;
using SternGarage.Services.Contracts;

namespace SternGarage.Controllers
{
    public class CarsController : Controller
    {
        private readonly ICarService _carService;

        public CarsController(ICarService carService)
        {
            _carService = carService;
        }

       [HttpGet]
        public async Task<IActionResult> Index(int? classId, string? searchTerm, string? sortBy)
        {
            var cars = await _carService.GetAllCarsAsync(classId, searchTerm, sortBy);

            ViewBag.Classes = await _carService.GetAllClassesAsync();
            ViewBag.CurrentClassId = classId;
            ViewBag.CurrentSearchTerm = searchTerm;
            ViewBag.CurrentSortBy = sortBy;

            return View(cars);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var viewModel = await _carService.GetCarDetailsByIdAsync(id.Value);

            if (viewModel == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create()
        {
            var viewModel = await _carService.GetCarForCreateAsync();
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CarFormViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                await _carService.AddCarAsync(viewModel);
                TempData["SuccessMessage"] = "Автомобилът е добавен!";
                return RedirectToAction(nameof(Index));
            }

            viewModel.Classes = await _carService.GetClassSelectListAsync();
            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var viewModel = await _carService.GetCarForEditAsync(id.Value);
            if (viewModel == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CarFormViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _carService.UpdateCarAsync(viewModel);
                TempData["SuccessMessage"] = "Автомобилът е редактиран!";
                return RedirectToAction(nameof(Details), new { id = viewModel.Id });
            }

            viewModel.Classes = await _carService.GetClassSelectListAsync();
            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _carService.GetCarForDeleteAsync(id.Value);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _carService.DeleteCarAsync(id);
            TempData["SuccessMessage"] = "Автомобилът е изтрит.";
            return RedirectToAction(nameof(Index));
        }
    }
}
