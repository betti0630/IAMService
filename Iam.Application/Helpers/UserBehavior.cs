using Iam.Application.Features.Base;

using MediatR;

using Microsoft.AspNetCore.Http;

using System.Security.Claims;


namespace Iam.Application.Helpers;

public class UserBehavior<TRequest, TResponse>(IHttpContextAccessor httpContextAccessor) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : UserRequest
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(next);

        var httpContext = httpContextAccessor.HttpContext;
        var userId = httpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (int.TryParse(userId, out int uid))
        {
            request.UserId = uid;
        }
        request.UserName = httpContext?.User?.FindFirstValue(ClaimTypes.Name);

        return await next(cancellationToken);
    }
}
