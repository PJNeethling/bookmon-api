using Bookmon.Domain.Entities;
using Bookmon.Domain.Interfaces;
using FluentValidation;

namespace Bookmon.Domain.Services;

public sealed class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRespository;
    private readonly IValidator<Order> _orderValidator;

    public OrderService(IOrderRepository orderRespository, IValidator<Order> orderValidator)
    {
        _orderRespository = orderRespository ?? throw new ArgumentNullException(nameof(orderRespository));
        _orderValidator = orderValidator ?? throw new ArgumentNullException(nameof(orderValidator));
    }

    public async Task<Order> CreateAsync(Order order)
    {
        await _orderValidator.ValidateAndThrowAsync(order);

        return await _orderRespository.CreateAsync(order);
    }

    public async Task<Order> GetAsync(Guid id, Guid userId)
    {
        return await _orderRespository.GetAsync(id, userId);
    }

    public async Task<IEnumerable<Order>> GetAllAsync(Guid userId)
    {
        return await _orderRespository.GetAllAsync(userId);
    }
}