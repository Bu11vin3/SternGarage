using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using static SternGarage.Common.ValidationConstants;

namespace SternGarage.Models
{
    public class Car
    {
        [Key]
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

        [ForeignKey("ClassId")]
        public virtual CarClass? Class { get; set; }

        [Required]
        [Range(CarHorsepowerMin, CarHorsepowerMax)]
        public int Horsepower { get; set; }

        [Required]
        [StringLength(CarEngineTypeMaxLength)]
        [Display(Name = "Тип двигател")]
        public string EngineType { get; set; } = null!;

        [Required]
        [Range(typeof(decimal), CarPriceMin, CarPriceMax)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [StringLength(CarDescriptionMaxLength)]
        public string? Description { get; set; }

        [Url]
        public string? ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
