using Bookmon.Domain.Entities;

namespace Bookmon.Domain.Interfaces;

public interface IOrderService
{
    Task<Order> CreateAsync(Order order);

    Task<Order> GetAsync(Guid id, Guid userId);

    Task<IEnumerable<Order>> GetAllAsync(Guid userId);
}