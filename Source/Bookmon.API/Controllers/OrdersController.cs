using Bookmon.API.Models.Requests;
using Bookmon.Domain.Entities;
using Bookmon.Domain.Interfaces;
using Bookmon.Domain.Validators.Constants;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Bookmon.API.Controllers;

[ApiController]
[Authorize]
[Route(RouteHelper.BaseRoute)]
public class OrdersController : BaseController
{
    private readonly IOrderService _orderService;
    private readonly IValidator<OrderRequest> _orderRequestValidator;

    public OrdersController(UserManager<User> userManager, ILogger<OrdersController> logger, IOrderService orderService, IValidator<OrderRequest> orderRequestValidator) : base(userManager, logger)
    {
        _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
        _orderRequestValidator = orderRequestValidator ?? throw new ArgumentNullException(nameof(orderRequestValidator));
    }

    [HttpPost]
    [SwaggerOperation(Tags = ["Orders"])]
    public async Task<IActionResult> PurchaseOrder(OrderRequest request)
    {
        await _orderRequestValidator.ValidateAndThrowAsync(request);

        if (!TryGetUser(out var user))
        {
            return Unauthorized($"Unauthorized: User {ValidationMessages.IsInvalid}");
        }

        var input = new Order(Guid.Parse(user.Id), request.Books);
        var response = await _orderService.CreateAsync(input);

        return Ok(response);
    }

    [HttpGet(RouteHelper.Id)]
    [SwaggerOperation(Tags = ["Orders"])]
    public async Task<IActionResult> GetOrder(Guid id)
    {
        //Add possible caching
        if (!TryGetUser(out var user))
        {
            return Unauthorized($"Unauthorized: User {ValidationMessages.IsInvalid}");
        }

        var book = await _orderService.GetAsync(id, Guid.Parse(user.Id));

        if (book is null)
        {
            return NotFound();
        }

        return Ok(book);
    }

    [HttpGet("user")]
    [SwaggerOperation(Tags = ["Orders"])]
    public async Task<IActionResult> GetOrders()
    {
        if (!TryGetUser(out var user))
        {
            return Unauthorized($"Unauthorized: User {ValidationMessages.IsInvalid}");
        }

        var orders = await _orderService.GetAllAsync(Guid.Parse(user.Id));

        if (orders is null || !orders.Any())
        {
            return Ok(Enumerable.Empty<Order>());
        }

        return Ok(orders);
    }
}