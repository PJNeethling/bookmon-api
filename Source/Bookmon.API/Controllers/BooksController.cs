using Bookmon.Domain.Entities;
using Bookmon.Domain.Interfaces;
using Bookmon.Domain.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookmon.API.Controllers;

[ApiController]
[Authorize]
[Route(RouteHelper.BaseRoute)]
public class BooksController : BaseController
{
    private readonly ILogger<BooksController> _logger;
    private readonly IBookService _bookService;
    private readonly IDistributedCache _cache;
    private readonly DistributedCacheEntryOptions _cacheOptions = new() { AbsoluteExpiration = DateTimeOffset.UtcNow.AddSeconds(20) };

    public BooksController(UserManager<User> userManager, ILogger<BooksController> logger, IBookService bookService, IDistributedCache cache) : base(userManager, logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
    }

    [HttpGet]
    [SwaggerOperation(Tags = ["Books"])]
    public async Task<IActionResult> GetBooks()
    {
        var cacheKey = "bookmon_books";
        var cachedBooks = await _cache.GetAsync(cacheKey);

        if (cachedBooks is not null && cachedBooks.Length > 0)
        {
            return Ok(cachedBooks.DeserializeThroughJson<IEnumerable<Book>>());
        }

        var books = await _bookService.GetAllAsync();

        if (books is null || !books.Any())
        {
            return Ok(Enumerable.Empty<Book>());
        }

        await _cache.SetAsync(cacheKey, books.SerializeThroughJson(), _cacheOptions);
        return Ok(books);
    }

    [HttpGet(RouteHelper.Id)]
    [SwaggerOperation(Tags = ["Books"])]
    public async Task<IActionResult> GetBook(Guid id)
    {
        var cacheKey = $"bookmon_books_{id}";
        var cachedBooks = await _cache.GetAsync(cacheKey);

        if (cachedBooks is not null && cachedBooks.Length > 0)
        {
            return Ok(cachedBooks.DeserializeThroughJson<Book>());
        }

        var book = await _bookService.GetAsync(id);

        if (book is null)
        {
            _logger.LogInformation($"Book with ID {id} not found.");
            return Ok(new Book());
        }

        await _cache.SetAsync(cacheKey, book.SerializeThroughJson(), _cacheOptions);
        return Ok(book);
    }
}