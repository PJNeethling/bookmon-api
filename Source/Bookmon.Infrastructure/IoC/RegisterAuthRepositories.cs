using Bookmon.Domain.Entities;
using Bookmon.Infrastructure.EntityFramework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Bookmon.Infrastructure.IoC;

public static class RegisterAuthRepositories
{
    public static void ConfigureAuthDatabase(this IServiceCollection services)
    {
        services.AddDbContext<AuthDbContext>(options => options.UseInMemoryDatabase("AuthDb"));
        services.AddScoped<AuthDbContext>();
    }

    public static void SeedAuthDatabase(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

        var users = new List<User>
                {
                    new User { UserName = "pj", Email = "pj@example.com", EmailConfirmed = true },
                    new User { UserName = "isac", Email = "isac@example.com", EmailConfirmed = true },
                    new User { UserName = "emi", Email = "emi@example.com" , EmailConfirmed = true}
                };

        foreach (var user in users)
        {
            userManager.CreateAsync(user, "P@ssw0rd").GetAwaiter().GetResult();
        }
    }
}