using Iam.Domain.Common;

using System.ComponentModel.DataAnnotations;

namespace Iam.Domain;

public class AppUser : AppEntity
{
    [Required]
    [MaxLength(100)]
    public string UserName { get; set; } = default!;
    [Required]
    public string PasswordHash { get; set; } = default!;
    public string? RolesCsv { get; set; }
    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = default!;
    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = default!;
    [Required]
    [MaxLength(100)]
    public string Email { get; set; } = default!;
    [Required]
    public bool ValidatedEmail { get; set;} = false;

}
