using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SternGarages.Data;
using System.Security.Claims;

[Authorize]
public class FavoritesController : Controller
{
    private readonly ApplicationDbContext _context;

    public FavoritesController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var favorites = await _context.Favorites
            .Include(f => f.Car)
            .Where(f => f.UserId == userId)
            .ToListAsync();

        return View(favorites);
    }

    [HttpPost]
    public async Task<IActionResult> Add(int carId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var exists = await _context.Favorites
            .AnyAsync(f => f.CarId == carId && f.UserId == userId);

        if (!exists)
        {
            var favorite = new Favorite
            {
                CarId = carId,
                UserId = userId!
            };

            _context.Favorites.Add(favorite);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Index", "Cars");
    }

    [HttpPost]
    public async Task<IActionResult> Remove(int id)
    {
        var favorite = await _context.Favorites.FindAsync(id);

        if (favorite != null)
        {
            _context.Favorites.Remove(favorite);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}