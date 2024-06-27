using AutoFixture.Xunit2;
using Bookmon.Domain.Entities;
using Bookmon.Domain.Interfaces;
using Bookmon.Domain.Services;
using Moq;

namespace Bookmon.Domain.Tests;

public class BookServiceTests
{
    private readonly IBookService _service;
    private readonly Mock<IBookRepository> _mockBookRepository;

    public BookServiceTests()
    {
        _mockBookRepository = new Mock<IBookRepository>();
        _service = new BookService(_mockBookRepository.Object);
    }

    [Theory, AutoData]
    public async Task Get_ValidInputs_ReturnsOrder(
        Guid id,
        Book response)
    {
        //Arrange
        response.Id = id;
        _mockBookRepository.Setup(x => x.GetAsync(id)).ReturnsAsync(response);

        //Act
        var result = await _service.GetAsync(id);

        //Assert
        _mockBookRepository.Verify(x => x.GetAsync(It.Is<Guid>(s => s == id)));
        Assert.Equal(response.Id, result.Id);
        Assert.Equal(response.Title, result.Title);
    }

    [Theory, AutoData]
    public async Task GetAll_ValidInputs_ReturnsSkuSets(
        List<Book> response)
    {
        //Arrange
        _mockBookRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(response);

        //Act
        var result = await _service.GetAllAsync();

        //Assert
        _mockBookRepository.Verify(x => x.GetAllAsync(), Times.Once);
        Assert.Equal(response.First().Id, result.First().Id);
        Assert.Equal(response.First().Title, result.First().Title);
    }
}