namespace Iam.Application.Contracts.Identity;

public interface IAuthService
{
    public Task<bool> ValidateUser(string userName, string password, CancellationToken cancellationToken = default);
    public Task<(string token, DateTime expires)> GenerateJwtToken(string userName);
}
