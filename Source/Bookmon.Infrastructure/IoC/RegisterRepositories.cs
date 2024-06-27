using Bookmon.Domain.Constants;
using Bookmon.Domain.Interfaces;
using Bookmon.Infrastructure.EntityFramework;
using Bookmon.Infrastructure.Extensions;
using Bookmon.Infrastructure.Mappers;
using Bookmon.Infrastructure.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bookmon.Infrastructure.IoC;

public static class RegisterRepositories
{
    public static void AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();

        services.AddDbContext<CosmosDbContext>(options =>
        {
            options.UseCosmos(
                configuration[ConfigurationNames.CosmosAccountEndpoint],
                configuration[ConfigurationNames.CosmosAccountKey],
                configuration[ConfigurationNames.CosmosDatabaseName]);
        });

        services.AddAutoMapper(typeof(BookMappingProfile));
    }

    public static void SeedCosmosDatabase(this IApplicationBuilder app)
    {
        using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetService<CosmosDbContext>();

            // Clear existing data
            context.ClearContainer<BookDto>();

            // Seed new data
            context.SeedData();
        }
    }
}