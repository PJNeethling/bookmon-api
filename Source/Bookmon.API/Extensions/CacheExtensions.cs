using Bookmon.Domain.Constants;
using Bookmon.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Bookmon.API.Extensions;

public static class CacheExtensions
{
    public static void AddCache(this IServiceCollection services, IConfiguration configuration)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        var cacheOptions = new CacheOptions();
        configuration.GetSection(ConfigurationNames.InMemoryCache).Bind(cacheOptions);

        if (cacheOptions.Enabled)
        {
            services.AddDistributedMemoryCache();
        }

        services.Configure<CacheOptions>(configuration.GetSection(ConfigurationNames.InMemoryCache));
    }
}