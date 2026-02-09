using System.ComponentModel.DataAnnotations;
using static MercedesBlog.Common.ValidationConstants;

namespace MercedesBlog.Models
{
    public class CarClass
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(ClassNameMaxLength, MinimumLength = ClassNameMinLength)]
        public string Name { get; set; } = null!;

        [StringLength(ClassDescriptionMaxLength)]
        public string? Description { get; set; }

        public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
    }
}
