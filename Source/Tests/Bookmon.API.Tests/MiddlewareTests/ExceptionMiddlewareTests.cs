using Bookmon.API.Middleware;
using Bookmon.Domain.Enums;
using Bookmon.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using System.Net;

namespace Bookmon.API.Tests.MiddlewareTests;

public sealed class ExceptionMiddlewareTests
{
    private readonly HttpContext _context = Substitute.For<HttpContext>();

    [Fact]
    public void Constructor_When_Called_Should_Succeed()
    {
        //arrange
        var requestDelegate = new RequestDelegate(TestDelegate);

        //act
        var instance = new ExceptionMiddleware(requestDelegate);

        //assert
        Assert.NotNull(instance);
    }

    [Fact]
    public void InvokeAsync_Returns_Expected()
    {
        //arrange
        var requestDelegate = new RequestDelegate(TestDelegate);
        var instance = new ExceptionMiddleware(requestDelegate);

        //act
        var result = instance.InvokeAsync(_context);

        //assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task InvokeAsync_Returns_InternalServerError_Status_With_General_Exception()
    {
        //arrange
        var requestDelegate = new RequestDelegate(TestDelegateWithException);
        var instance = new ExceptionMiddleware(requestDelegate);

        //act
        await instance.InvokeAsync(_context);

        //assert
        Assert.Equal(HttpStatusCode.InternalServerError, (HttpStatusCode)_context.Response.StatusCode);
    }

    [Fact]
    public async Task InvokeAsync_Returns_Expected_Status_Code_With_DomainException()
    {
        //arrange
        var requestDelegate = new RequestDelegate(TestDelegateWithDomainException);
        var instance = new ExceptionMiddleware(requestDelegate);

        //act
        await instance.InvokeAsync(_context);

        //assert
        Assert.Equal(HttpStatusCode.BadRequest, (HttpStatusCode)_context.Response.StatusCode);
    }

    private Task TestDelegate(HttpContext context)
    {
        return Task.CompletedTask;
    }

    private static Task TestDelegateWithException(HttpContext context)
    {
        throw new Exception();
    }

    private static Task TestDelegateWithDomainException(HttpContext context)
    {
        throw new DomainException(DomainExceptionCodes.EntityAlreadyExists);
    }
}