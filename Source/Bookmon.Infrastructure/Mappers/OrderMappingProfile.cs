using AutoMapper;
using Bookmon.Domain.Entities;
using Bookmon.Infrastructure.Models;

namespace Bookmon.Infrastructure.Mappers;

public sealed class OrderMappingProfile : Profile
{
    public OrderMappingProfile()
    {
        CreateMap<OrderDto, Order>();
    }
}