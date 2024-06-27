using Bookmon.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace Bookmon.API.Controllers;

public class BaseController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<BaseController> _logger;

    public BaseController(UserManager<User> userManager, ILogger<BaseController> logger)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected bool TryGetUser(out User user)
    {
        user = GetCurrentUserAsync();

        if (user is null)
        {
            return false;
        }

        return true;
    }

    protected User GetCurrentUserAsync()
    {
        try
        {
            var user = _userManager.GetUserAsync(HttpContext.User);

            if (user == null)
            {
                _logger.LogWarning("Failed to retrieve current user: User is not authenticated.");
            }

            return user.Result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving the current user.");
            throw;
        }
    }
}