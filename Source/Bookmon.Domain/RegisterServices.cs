using Bookmon.Domain.Interfaces;
using Bookmon.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Bookmon.Domain;

public static class RegisterServices
{
    public static void AddDomainServices(this IServiceCollection services)
    {
        services.AddTransient<IBookService, BookService>();
        services.AddTransient<IOrderService, OrderService>();
    }
}