using AutoMapper;
using Bookmon.Domain.Entities;
using Bookmon.Infrastructure.Models;

namespace Bookmon.Infrastructure.Mappers;

public sealed class BookMappingProfile : Profile
{
    public BookMappingProfile()
    {
        CreateMap<BookDto, Book>();
    }
}