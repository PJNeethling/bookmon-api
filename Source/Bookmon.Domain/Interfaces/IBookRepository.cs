using Bookmon.Domain.Entities;

namespace Bookmon.Domain.Interfaces;

public interface IBookRepository
{
    Task<IEnumerable<Book>> GetAllAsync();

    Task<Book> GetAsync(Guid id);
}