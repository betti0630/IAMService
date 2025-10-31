using System.ComponentModel.DataAnnotations;

namespace Iam.Application.Identity.Dtos.Auth;

public record LoginRequest
{
    public LoginRequest(string @userName, string @password)
    {
        UserName = @userName;
        Password = @password;
    }

    [Required(AllowEmptyStrings = true)]
    public string UserName { get; set; }

    [Required(AllowEmptyStrings = true)]
    public string Password { get; set; }

}
