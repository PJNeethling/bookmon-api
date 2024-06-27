using AutoMapper;
using Bookmon.Domain.Entities;
using Bookmon.Domain.Enums;
using Bookmon.Domain.Exceptions;
using Bookmon.Domain.Interfaces;
using Bookmon.Infrastructure.EntityFramework;
using Bookmon.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Bookmon.Infrastructure;

public sealed class OrderRepository : IOrderRepository
{
    protected readonly CosmosDbContext _cosmosDbContext;
    private readonly IMapper _mapper;

    public OrderRepository(CosmosDbContext cosmosDbContext, IMapper mapper)
    {
        _cosmosDbContext = cosmosDbContext ?? throw new ArgumentNullException(nameof(cosmosDbContext));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Order> CreateAsync(Order order)
    {
        try
        {
            foreach (var bookId in order.Books)
            {
                var existingBook = await _cosmosDbContext.Books.FindAsync(bookId) ?? throw new DomainException(DomainExceptionCodes.EntityNotFound, $"Book not found");
            }

            var orderDto = new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                Books = order.Books.Select(x => x.ToString()).ToList(),
                CreatedDate = order.CreatedDate
            };

            var entityEntry = _cosmosDbContext.Orders.Add(orderDto);

            await _cosmosDbContext.SaveChangesAsync();

            var result = _mapper.Map<Order>(entityEntry.Entity);

            return result;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Order> GetAsync(Guid id, Guid userId)
    {
        var order = _cosmosDbContext.Orders.Where(x => x.Id == id && x.UserId == userId).AsNoTracking().FirstOrDefault();

        return order is null ? throw new DomainException(DomainExceptionCodes.EntityNotFound, "Order not found") : await Task.FromResult(_mapper.Map<Order>(order));
    }

    public async Task<IEnumerable<Order>> GetAllAsync(Guid userId)
    {
        var orders = await _cosmosDbContext.Orders
                              .Where(x => x.UserId == userId)
                              .AsNoTracking()
                              .ToListAsync();

        return _mapper.Map<IEnumerable<Order>>(orders);
    }
}