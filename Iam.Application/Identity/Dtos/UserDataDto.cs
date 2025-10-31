namespace Iam.Application.Identity.Dtos;

public class UserDataDto
{
    public int Id { get; set; }
    public string UserName { get; set; } = null!;
    public string? RolesCsv { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
}
