using Iam.Domain;
using Iam.Domain.Common;

using Microsoft.EntityFrameworkCore;

namespace Iam.Persistence.DatabaseContext;

internal sealed class AuthDbContext : DbContext
{
    //private readonly IAuthUserService _authService;

    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
        //_authService = authService;
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>()
            .Where(q => q.State == EntityState.Added || q.State == EntityState.Modified))
        {
            entry.Entity.DateModified = DateTime.Now;
            //entry.Entity.ModifiedBy = _authService.UserName;
            if (entry.State == EntityState.Added)
            {
                entry.Entity.DateCreated = DateTime.Now;
                //entry.Entity.CreatedBy = _authService.UserName;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<AppUser> AppUsers => Set<AppUser>(); 
 }
    
