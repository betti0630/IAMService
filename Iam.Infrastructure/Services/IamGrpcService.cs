using Grpc.Common.Iam;

using Grpc.Core;

using Iam.Application.Contracts.Identity;


namespace Iam.Infrastructure.Services;

public class IamGrpcService : IamService.IamServiceBase
{
    private readonly IAppUserService _appUserService;

    public IamGrpcService(IAppUserService appUserService)
    {
        _appUserService = appUserService;
    }

    public override Task<PingResponse> Ping(PingRequest request, ServerCallContext context)
    {
        return Task.FromResult(new PingResponse { Success = true });
    }

    public override async Task<GetUserIdByUserNameResponse> GetUserIdByUserName(GetUserIdByUserNameRequest request, ServerCallContext context)
    {
        ArgumentNullException.ThrowIfNull(request);
        var userId = await _appUserService.GetUserIdByUserName(request.UserName);
        return new GetUserIdByUserNameResponse
        {
            UserId = userId,
        };
    }

    public override async Task<GetUserDataResponse> GetUserData(GetUserDataRequest request, ServerCallContext context)
    {
        ArgumentNullException.ThrowIfNull(request);
        var userData = await _appUserService.GetUserData(request.UserId);
        return new GetUserDataResponse
        {
            UserId = userData.Id,
            UserName = userData.UserName,
            Roles = userData.RolesCsv,
            FirstName = userData.FirstName,
            LastName = userData.LastName,
            Email = userData.Email
        };
    }

}
