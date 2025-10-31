using Iam.Application.Contracts.Persistence;
using Iam.Domain;
using Iam.Persistence.DatabaseContext;

using Microsoft.EntityFrameworkCore;

namespace Iam.Persistence.Repositories;

internal sealed class AppUserRepository : GenericRepository<AppUser>, IGenericRepository<AppUser>
{

    public AppUserRepository(IDbContextFactory<AuthDbContext> contextFactory) : base(contextFactory)
    {
    }

}
