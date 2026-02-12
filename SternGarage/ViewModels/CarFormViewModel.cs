using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using static SternGarage.Common.ValidationConstants;

namespace SternGarage.ViewModels
{
    public class CarFormViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(CarModelMaxLength, MinimumLength = CarModelMinLength)]
        public string Model { get; set; } = null!;

        [Required]
        [Range(CarYearMin, CarYearMax)]
        public int Year { get; set; }

        [Required]
        [Display(Name = "Клас")]
        public int ClassId { get; set; }

        [Required]
        [Range(CarHorsepowerMin, CarHorsepowerMax)]
        public int Horsepower { get; set; }

        [Required]
        [StringLength(CarEngineTypeMaxLength)]
        public string EngineType { get; set; } = null!;

        [Required]
        [Range(typeof(decimal), CarPriceMin, CarPriceMax)]
        public decimal Price { get; set; }

        [StringLength(CarDescriptionMaxLength)]
        public string? Description { get; set; }

        [Url]
        public string? ImageUrl { get; set; }

        public IEnumerable<SelectListItem>? Classes { get; set; }
    }
}
