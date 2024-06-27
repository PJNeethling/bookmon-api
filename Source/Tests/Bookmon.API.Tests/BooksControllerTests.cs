using AutoFixture.Xunit2;
using Bookmon.API.Controllers;
using Bookmon.Domain.Entities;
using Bookmon.Domain.Interfaces;
using Bookmon.Domain.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;

namespace Bookmon.API.Tests;

public sealed class BooksControllerTests
{
    private readonly Mock<IDistributedCache> _mockCache;
    private readonly Mock<ILogger<BooksController>> _mockLogger;
    private readonly Mock<IBookService> _mockBookService;
    private readonly Mock<UserManager<User>> _mockUserManager;
    private readonly BooksController _controller;

    public BooksControllerTests()
    {
        _mockCache = new Mock<IDistributedCache>();
        _mockLogger = new Mock<ILogger<BooksController>>();
        _mockBookService = new Mock<IBookService>();
        var mockUserStore = new Mock<IUserStore<User>>();
        _mockUserManager = new Mock<UserManager<User>>(mockUserStore.Object, null, null, null, null, null, null, null, null);
        _controller = new BooksController(_mockUserManager.Object, _mockLogger.Object, _mockBookService.Object, _mockCache.Object);
    }

    [Theory, AutoData]
    public async Task GetBooks_Returns_Ok_With_Cached_Books(List<Book> cachedBooks)
    {
        // Arrange
        var serializedBooks = cachedBooks.SerializeThroughJson();
        _mockCache.Setup(c => c.GetAsync("bookmon_books", default)).ReturnsAsync(serializedBooks);

        // Act
        var result = await _controller.GetBooks();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var books = Assert.IsAssignableFrom<IEnumerable<Book>>(okResult.Value);
        Assert.Equal(cachedBooks.First().Id, books.First().Id);
    }

    [Theory, AutoData]
    public async Task GetBooks_Returns_Ok_With_Books_From_Service(List<Book> mockBooks)
    {
        // Arrange
        _mockCache.Setup(c => c.GetAsync("bookmon_books", default)).ReturnsAsync((byte[])null);
        _mockBookService.Setup(s => s.GetAllAsync()).ReturnsAsync(mockBooks);

        // Act
        var result = await _controller.GetBooks();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var books = Assert.IsAssignableFrom<IEnumerable<Book>>(okResult.Value);
        Assert.Equal(mockBooks, books);
    }

    [Fact]
    public async Task GetBooks_Returns_Ok_With_Null_Books_From_Service()
    {
        // Arrange
        _mockCache.Setup(c => c.GetAsync("bookmon_books", default)).ReturnsAsync((byte[])null);
        _mockBookService.Setup(s => s.GetAllAsync()).ReturnsAsync((List<Book>)null);

        // Act
        var result = await _controller.GetBooks();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var books = Assert.IsAssignableFrom<IEnumerable<Book>>(okResult.Value);
        Assert.Empty(books);
    }

    [Theory, AutoData]
    public async Task GetBook_Returns_Ok_With_Cached_Book(Book cachedBook, Guid bookId)
    {
        var serializedBook = cachedBook.SerializeThroughJson();
        _mockCache.Setup(c => c.GetAsync($"bookmon_books_{bookId}", default)).ReturnsAsync(serializedBook);

        // Act
        var result = await _controller.GetBook(bookId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var book = Assert.IsAssignableFrom<Book>(okResult.Value);
        Assert.Equal(cachedBook.Id, book.Id);
    }

    [Theory, AutoData]
    public async Task GetBook_Returns_Ok_With_Book_From_Service(Book mockBook, Guid bookId)
    {
        // Arrange
        _mockCache.Setup(c => c.GetAsync($"bookmon_books_{bookId}", default)).ReturnsAsync((byte[])null);
        _mockBookService.Setup(s => s.GetAsync(bookId)).ReturnsAsync(mockBook);

        // Act
        var result = await _controller.GetBook(bookId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var book = Assert.IsAssignableFrom<Book>(okResult.Value);
        Assert.Equal(mockBook, book);
    }

    [Theory, AutoData]
    public async Task GetBook_Returns_Ok_With_Null_Book_From_Service(Guid bookId)
    {
        // Arrange
        _mockCache.Setup(c => c.GetAsync($"bookmon_books_{bookId}", default)).ReturnsAsync((byte[])null);
        _mockBookService.Setup(s => s.GetAsync(bookId)).ReturnsAsync((Book)null);

        // Act
        var result = await _controller.GetBook(bookId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var book = Assert.IsAssignableFrom<Book>(okResult.Value);
        Assert.Equal(Guid.Empty, book.Id);
    }
}