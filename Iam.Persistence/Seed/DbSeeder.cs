
using Microsoft.EntityFrameworkCore;
using Iam.Persistence.DatabaseContext;
using Iam.Application.Contracts.Identity;

namespace Iam.Persistence.Seed;

internal sealed class DbSeeder(AuthDbContext dbContext, IAppUserService userService) : IDbSeeder
{
    public async Task SeedAsync()
    {
        if (await dbContext.AppUsers.AnyAsync())
        {
            return; // DB has been seeded
        }
        var users = new List<(string UserName, string Password, string FirstName, string LastName, string email, string Role)>
        {
            ("alice", "Passw0rd!", "Alice", "Johnson", "testalice@mail.com", "User"),
            ("bob", "Passw0rd!", "Bob", "Miller", "testbob@mail.com", "User"),
            ("admin", "AdminPassw0rd!", "Adam", "Novak", "admin@mail.com", "Admin")
        };
        
        foreach (var (userName, password, firstName, lastName, email, role) in users)
        {
            await userService.SeedNewUser(userName, password, firstName, lastName, email, role);
        }
        var Alice = await dbContext.AppUsers.FirstAsync(u => u.UserName == "alice");
        var Bob = await dbContext.AppUsers.FirstAsync(u => u.UserName == "bob");


  
        await dbContext.SaveChangesAsync();
    }


}
