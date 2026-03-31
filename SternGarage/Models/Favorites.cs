using SternGarage.Models;
using System.ComponentModel.DataAnnotations;

public class Favorite
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = null!;

    public virtual ApplicationUser? User { get; set; }

    [Required]
    public int CarId { get; set; }

    public virtual Car? Car { get; set; }
}