using Bookmon.Domain.Entities;
using Bookmon.Domain.Interfaces;

namespace Bookmon.Domain.Services;

public sealed class BookService : IBookService
{
    private readonly IBookRepository _bookRespository;

    public BookService(IBookRepository bookRespository)
    {
        _bookRespository = bookRespository ?? throw new ArgumentNullException(nameof(bookRespository));
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        return await _bookRespository.GetAllAsync();
    }

    public async Task<Book> GetAsync(Guid id)
    {
        return await _bookRespository.GetAsync(id);
    }
}