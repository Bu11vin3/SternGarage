using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static SternGarage.Common.ValidationConstants;

namespace SternGarage.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Автомобил")]
        public int CarId { get; set; }

        [ForeignKey("CarId")]
        public virtual Car? Car { get; set; }

        [Required]
        [StringLength(ReviewAuthorMaxLength, MinimumLength = ReviewAuthorMinLength)]
        public string AuthorName { get; set; } = null!;

        [Required]
        [StringLength(ReviewTitleMaxLength, MinimumLength = ReviewTitleMinLength)]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(ReviewContentMaxLength, MinimumLength = ReviewContentMinLength)]
        public string Content { get; set; } = null!;

        [Required]
        [Range(ReviewRatingMin, ReviewRatingMax)]
        public int Rating { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
