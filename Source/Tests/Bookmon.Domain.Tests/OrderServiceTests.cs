using AutoFixture.Xunit2;
using Bookmon.Domain.Entities;
using Bookmon.Domain.Interfaces;
using Bookmon.Domain.Services;
using FluentValidation;
using Moq;

namespace Bookmon.Domain.Tests;

public sealed class OrderServiceTests
{
    private readonly IOrderService _service;
    private readonly Mock<IOrderRepository> _mockOrderRepository;
    private readonly Mock<IValidator<Order>> _mockOrderValidator;

    public OrderServiceTests()
    {
        _mockOrderRepository = new Mock<IOrderRepository>();
        _mockOrderValidator = new Mock<IValidator<Order>>();

        _service = new OrderService(_mockOrderRepository.Object, _mockOrderValidator.Object);
    }

    [Theory, AutoData]
    public async Task Create_ValidInputs_ReturnsCreatedOrder(Guid userId, List<Guid> books)
    {
        //Arrange
        var request = new Order(userId, books);
        var response = new Order
        {
            Id = request.Id,
            UserId = request.UserId,
            Books = request.Books
        };
        _mockOrderRepository.Setup(x => x.CreateAsync(request)).ReturnsAsync(response);

        //Act
        var result = await _service.CreateAsync(request);

        //Assert

        //Validate that validator got called
        _mockOrderRepository.Verify(x => x.CreateAsync(It.Is<Order>(s =>
                    s.Id == request.Id
                    && s.UserId == request.UserId
                    && s.Books == request.Books
                    && s.Books.First() == request.Books.First())));

        _mockOrderValidator.Verify(x => x.ValidateAsync(It.Is<ValidationContext<Order>>(x => true),
            It.IsAny<CancellationToken>()), Times.Once);

        Assert.Equal(request.Id, result.Id);
    }

    [Theory, AutoData]
    public async Task Get_ValidInputs_ReturnsOrder(
        Guid id,
        Guid userId,
        Order response)
    {
        //Arrange
        response.Id = id;
        _mockOrderRepository.Setup(x => x.GetAsync(id, userId)).ReturnsAsync(response);

        //Act
        var result = await _service.GetAsync(id, userId);

        //Assert
        _mockOrderRepository.Verify(x => x.GetAsync(It.Is<Guid>(s => s == id), It.Is<Guid>(s => s == userId)));
        Assert.Equal(response.Id, result.Id);
        Assert.Equal(response.Books, result.Books);
    }

    [Theory, AutoData]
    public async Task GetAll_ValidInputs_ReturnsSkuSets(
        Guid userId,
        List<Order> response)
    {
        //Arrange
        _mockOrderRepository.Setup(x => x.GetAllAsync(userId)).ReturnsAsync(response);

        //Act
        var result = await _service.GetAllAsync(userId);

        //Assert
        _mockOrderRepository.Verify(x => x.GetAllAsync(userId), Times.Once);
        Assert.Equal(response.First().Id, result.First().Id);
        Assert.Equal(response.First().Books, result.First().Books);
    }
}