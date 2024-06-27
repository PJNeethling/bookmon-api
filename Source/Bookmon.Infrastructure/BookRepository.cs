using AutoMapper;
using Bookmon.Domain.Entities;
using Bookmon.Domain.Enums;
using Bookmon.Domain.Exceptions;
using Bookmon.Domain.Interfaces;
using Bookmon.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Bookmon.Infrastructure;

public sealed class BookRepository : IBookRepository
{
    protected readonly CosmosDbContext _cosmosDbContext;
    private readonly IMapper _mapper;

    public BookRepository(CosmosDbContext cosmosDbContext, IMapper mapper)
    {
        _cosmosDbContext = cosmosDbContext ?? throw new ArgumentNullException(nameof(cosmosDbContext));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        var books = _cosmosDbContext.Books.AsNoTracking().ToList();

        return await Task.FromResult(_mapper.Map<List<Book>>(books));
    }

    public async Task<Book> GetAsync(Guid id)
    {
        var book = _cosmosDbContext.Books.Where(x => x.Id == id).AsNoTracking().FirstOrDefault();

        return book is null ? throw new DomainException(DomainExceptionCodes.EntityNotFound, "Book not found") : await Task.FromResult(_mapper.Map<Book>(book));
    }
}