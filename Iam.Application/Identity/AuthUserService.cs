using Iam.Application.Contracts.Identity;
using Iam.Application.Models;
using Iam.Domain;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Iam.Application.Identity;

internal sealed class AuthUserService(IOptions<JwtSettings> jwtSettings, IHttpContextAccessor httpContextAccessor) : IAuthUserService
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

    public (string token, DateTime expires) GenerateJwtToken( AppUser user)
    {

        if (user is null) throw new InvalidOperationException("User not found");
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.NameIdentifier, user.Id.ToString(CultureInfo.InvariantCulture))
        };

        if (!string.IsNullOrWhiteSpace(user.RolesCsv))
        {
            foreach (var r in user.RolesCsv.Split(',', StringSplitOptions.RemoveEmptyEntries))
                claims.Add(new Claim(ClaimTypes.Role, r));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddHours(2);
        var token = new JwtSecurityToken(_jwtSettings.Issuer, _jwtSettings.Audience, claims, expires: expires, signingCredentials: creds);
        return (new JwtSecurityTokenHandler().WriteToken(token), expires);
    }

    public int? UserId
    {
        get
        {
            var userId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userId, out int uid)) {
                return uid;
            }
            return null;
        }
    }
    public string? UserName => httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);

}
