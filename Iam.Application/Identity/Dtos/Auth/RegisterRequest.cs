using System.ComponentModel.DataAnnotations;

namespace Iam.Application.Identity.Dtos.Auth;

public record RegisterRequest
{
    public RegisterRequest(string @userName, string @password, string firstName, string lastName, string email )
    {
        UserName = @userName;
        Password = @password;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    [Required]
    [StringLength(int.MaxValue, MinimumLength = 3)]
    public string UserName { get; }

    [Required]
    [StringLength(int.MaxValue, MinimumLength = 6)]
    public string Password { get; }

    [Required]
    [StringLength(100,  MinimumLength = 3)]
    public string FirstName { get; }

    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string LastName { get; }

    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Email { get; }
}
