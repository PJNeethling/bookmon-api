using AutoFixture.Xunit2;
using Bookmon.API.Controllers;
using Bookmon.API.Models.Requests;
using Bookmon.Domain.Entities;
using Bookmon.Domain.Interfaces;
using Bookmon.Domain.Validators.Constants;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;

namespace Bookmon.API.Tests;

public sealed class OrdersControllerTests
{
    private readonly Mock<IDistributedCache> _mockCache;
    private readonly Mock<ILogger<OrdersController>> _mockLogger;
    private readonly Mock<IOrderService> _mockOrderService;
    private readonly Mock<IValidator<OrderRequest>> _mockOrderRequestValidator;

    private readonly Mock<UserManager<User>> _mockUserManager;
    private readonly OrdersController _controller;

    public OrdersControllerTests()
    {
        _mockCache = new Mock<IDistributedCache>();
        _mockLogger = new Mock<ILogger<OrdersController>>();
        _mockOrderService = new Mock<IOrderService>();
        _mockOrderRequestValidator = new Mock<IValidator<OrderRequest>>();

        var mockUserStore = new Mock<IUserStore<User>>();
        _mockUserManager = new Mock<UserManager<User>>(mockUserStore.Object, null, null, null, null, null, null, null, null);
        _controller = new OrdersController(_mockUserManager.Object, _mockLogger.Object, _mockOrderService.Object, _mockOrderRequestValidator.Object);
    }

    [Fact]
    public async Task PurchaseOrder_Returns_Unauthorized_When_User_Not_Authenticated()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        var controllerContext = new ControllerContext { HttpContext = httpContext };
        _controller.ControllerContext = controllerContext;

        _mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync((User)null);

        // Act
        var result = await _controller.PurchaseOrder(new OrderRequest());

        // Assert
        var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
        Assert.Equal($"Unauthorized: User {ValidationMessages.IsInvalid}", unauthorizedResult.Value);
    }

    [Theory, AutoData]
    public async Task PurchaseOrder_Returns_Ok_When_Order_Creation_Successful(OrderRequest request, Order order)
    {
        // Arrange
        var user = new User { Id = Guid.NewGuid().ToString() };
        var userClaims = new List<Claim> { new Claim(ClaimTypes.Name, user.Id) };
        var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(userClaims));
        var httpContext = new DefaultHttpContext { User = userPrincipal };
        var controllerContext = new ControllerContext { HttpContext = httpContext };
        _controller.ControllerContext = controllerContext;

        order.Books = request.Books;

        _mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
        _mockOrderService.Setup(os => os.CreateAsync(It.IsAny<Order>())).ReturnsAsync(order);

        // Act
        var result = await _controller.PurchaseOrder(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var orderResult = Assert.IsAssignableFrom<Order>(okResult.Value);
        _mockOrderRequestValidator.Verify(x => x.ValidateAsync(It.Is<ValidationContext<OrderRequest>>(x => true),
            It.IsAny<CancellationToken>()), Times.Once);

        Assert.IsType<Order>(okResult.Value);
        Assert.Equal(request.Books, orderResult.Books);
    }

    [Theory, AutoData]
    public async Task GetOrder_Returns_Unauthorized_When_User_Not_Authenticated(Guid orderId)
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        var controllerContext = new ControllerContext { HttpContext = httpContext };
        _controller.ControllerContext = controllerContext;

        // Act
        var result = await _controller.GetOrder(orderId);

        // Assert
        var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
        Assert.Equal($"Unauthorized: User {ValidationMessages.IsInvalid}", unauthorizedResult.Value);
    }

    [Theory, AutoData]
    public async Task GetOrder_Returns_NotFound_When_Order_Not_Found(Guid orderId)
    {
        // Arrange
        var user = new User { Id = Guid.NewGuid().ToString() };
        var userClaims = new List<Claim> { new Claim(ClaimTypes.Name, user.Id) };
        var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(userClaims));
        var httpContext = new DefaultHttpContext { User = userPrincipal };
        var controllerContext = new ControllerContext { HttpContext = httpContext };
        _controller.ControllerContext = controllerContext;

        _mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
        _mockOrderService.Setup(os => os.GetAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync((Order)null);

        // Act
        var result = await _controller.GetOrder(orderId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Theory, AutoData]
    public async Task GetOrder_Returns_Ok_When_Order_Found(Guid orderId, Order order)
    {
        // Arrange
        var user = new User { Id = Guid.NewGuid().ToString() };
        var userClaims = new List<Claim> { new Claim(ClaimTypes.Name, user.Id) };
        var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(userClaims));
        var httpContext = new DefaultHttpContext { User = userPrincipal };
        var controllerContext = new ControllerContext { HttpContext = httpContext };
        _controller.ControllerContext = controllerContext;

        _mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
        _mockOrderService.Setup(os => os.GetAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(order);

        // Act
        var result = await _controller.GetOrder(orderId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var orderResult = Assert.IsAssignableFrom<Order>(okResult.Value);
        Assert.IsType<Order>(okResult.Value);
        Assert.Equal(order.Id, orderResult.Id);
    }

    [Fact]
    public async Task GetOrders_Returns_Unauthorized_When_User_Not_Authenticated()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        var controllerContext = new ControllerContext { HttpContext = httpContext };
        _controller.ControllerContext = controllerContext;

        // Act
        var result = await _controller.GetOrders();

        // Assert
        var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
        Assert.Equal($"Unauthorized: User {ValidationMessages.IsInvalid}", unauthorizedResult.Value);
    }

    [Fact]
    public async Task GetOrders_Returns_EmptyList_When_No_Orders_Found()
    {
        // Arrange
        var user = new User { Id = Guid.NewGuid().ToString() };
        var userClaims = new List<Claim> { new Claim(ClaimTypes.Name, user.Id) };
        var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(userClaims));
        var httpContext = new DefaultHttpContext { User = userPrincipal };
        var controllerContext = new ControllerContext { HttpContext = httpContext };
        _controller.ControllerContext = controllerContext;

        _mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
        _mockOrderService.Setup(os => os.GetAllAsync(It.IsAny<Guid>())).ReturnsAsync(Enumerable.Empty<Order>());

        // Act
        var result = await _controller.GetOrders();

        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var orders = Assert.IsAssignableFrom<IEnumerable<Order>>(okResult.Value);
        Assert.Empty(orders);
    }

    [Theory, AutoData]
    public async Task GetOrders_Returns_Orders_When_Found(List<Order> orders)
    {
        // Arrange
        var user = new User { Id = Guid.NewGuid().ToString() };
        var userClaims = new List<Claim> { new Claim(ClaimTypes.Name, user.Id) };
        var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(userClaims));
        var httpContext = new DefaultHttpContext { User = userPrincipal };
        var controllerContext = new ControllerContext { HttpContext = httpContext };
        _controller.ControllerContext = controllerContext;

        _mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
        _mockOrderService.Setup(os => os.GetAllAsync(It.IsAny<Guid>())).ReturnsAsync(orders);

        // Act
        var result = await _controller.GetOrders();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedOrders = Assert.IsAssignableFrom<IEnumerable<Order>>(okResult.Value);
        Assert.Equal(orders.Count, returnedOrders.Count());
        Assert.Equal(orders.First().Id, returnedOrders.First().Id);
    }
}