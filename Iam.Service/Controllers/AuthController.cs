using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Iam.Application.Identity.Dtos.Auth;
using Iam.Application.Contracts.Identity;

namespace Iam.Service.Controllers;

[Route("api/[controller]")]
[ApiController]
#pragma warning disable CA1515 // Consider making public types internal
public class AuthController(IAuthService authService, IAppUserService userService) : ControllerBase
#pragma warning restore CA1515 // Consider making public types internal
{

    /// <summary>
    /// Login (JWT)
    /// </summary>
    /// <returns>Successful login</returns>
    [HttpPost, Route("login", Name = "login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponse>> Login([FromBody]LoginRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (!await authService.ValidateUser(request.UserName, request.Password, cancellationToken))
        {
            return Unauthorized();
        }
        var (token, expires) = await authService.GenerateJwtToken(request.UserName);
        return Ok(new LoginResponse(token));
    }

    /// <summary>
    /// UserDto registration
    /// </summary>
    /// <returns>Created</returns>
    [HttpPost, Route("register", Name = "register")]
    public async Task<ActionResult<string>> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        await userService.AddNewUser(request.UserName, request.Password, request.FirstName, request.LastName, request.Email, "User", cancellationToken);
        return CreatedAtRoute("register", request.UserName, "User created");    
    }

    [HttpGet("verify")]
    public async Task<ActionResult<int>> VerifyEmail(string token, CancellationToken cancellationToken)
    {
        var userId = await userService.MarkEmailAsVerified(token, cancellationToken);
        return Ok(userId);
    }
}


