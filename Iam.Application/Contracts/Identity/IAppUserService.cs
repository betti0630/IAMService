using Iam.Application.Identity.Dtos;

namespace Iam.Application.Contracts.Identity;

public interface IAppUserService
{
    Task SeedNewUser(string userName, string password, string firstName, string lastName, string email, string? roles, CancellationToken cancellationToken = default);
    Task AddNewUser(string userName, string password, string firstName, string lastName, string email, string? roles, CancellationToken cancellationToken = default);
    Task<int> GetUserIdByUserName(string userName, CancellationToken cancellationToken = default);
    Task<UserDataDto> GetUserData(int userId, CancellationToken cancellationToken = default);
    Task<int> MarkEmailAsVerified(string token, CancellationToken cancellationToken = default);
}
