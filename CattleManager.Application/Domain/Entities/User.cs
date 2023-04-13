using System.ComponentModel.DataAnnotations;
namespace CattleManager.Application.Domain.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(255)]
    public string FirstName { get; set; } = null!;

    [Required]
    [MaxLength(255)]
    public string LastName { get; set; } = null!;

    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = null!;

    [Required]
    [MaxLength(255)]
    public string Password { get; set; } = null!;

    public virtual ICollection<Cattle> Cattle { get; set; } = null!;
}