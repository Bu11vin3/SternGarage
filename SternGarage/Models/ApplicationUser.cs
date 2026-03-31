using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SternGarage.Models
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(100)]
        public string? FullName { get; set; }

        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
