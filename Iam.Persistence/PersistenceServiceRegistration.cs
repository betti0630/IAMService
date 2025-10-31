using Iam.Application.Contracts.Persistence;
using Iam.Persistence.DatabaseContext;
using Iam.Persistence.Repositories;
using Iam.Persistence.Seed;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Iam.Persistence;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        //services.AddDbContext<AuthDbContext>(options =>
        //    options.UseMySql(
        //        configuration.GetConnectionString("DefaultConnection"),
        //        new MySqlServerVersion(new Version(11, 3, 0)),
        //        b => b.MigrationsAssembly(typeof(AuthDbContext).Assembly.FullName)
        //));

        services.AddDbContextFactory<AuthDbContext>(options =>
            options.UseMySql(
                configuration.GetConnectionString("DefaultConnection"),
                new MySqlServerVersion(new Version(11, 3, 0)),
                b => { 
                    b.MigrationsAssembly(typeof(AuthDbContext).Assembly.FullName); 
                    b.EnableStringComparisonTranslations(); 
                }
            )
         );

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IDbSeeder,DbSeeder>();
        return services;
    }

    public static void RunDatabaseMigrations(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<AuthDbContext>();

        var retries = 10;
        var delay = TimeSpan.FromSeconds(5);

        /// Try to apply migrations with retries, because the database might not be available yet.
        while (retries > 0)
        {
            if (db.Database.CanConnect())
            {
                if (!db.Database.GetPendingMigrations().Any())
                {
                    break;
                }
                var migrationCount = db.Database.GetPendingMigrations().Count();
                db.Database.Migrate();
                Console.WriteLine($"Migration is successful. There were {migrationCount} pending migrations");
                break;
            } else {
                retries--;
                Console.WriteLine($"Database not available yet. Retrying in {delay.TotalSeconds} seconds... ({retries} retries left)");
                Thread.Sleep(delay);
            } 
        }


    }

}
