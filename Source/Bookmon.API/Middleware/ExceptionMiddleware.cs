using Bookmon.API.Models.ExceptionErrors;
using Bookmon.API.Models.Responses;
using Bookmon.Domain.Enums;
using Bookmon.Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Bookmon.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (ValidationException validationException)
        {
            await HandleExceptionAsync(context, validationException, HttpStatusCode.BadRequest);
        }
        catch (DomainException domainException)
        {
            var statusCode = domainException.DomainExceptionCode switch
            {
                DomainExceptionCodes.EntityAlreadyExists => HttpStatusCode.BadRequest,
                DomainExceptionCodes.EntityNotFound => HttpStatusCode.NotFound,
                DomainExceptionCodes.BadRequest => HttpStatusCode.BadRequest,
                _ => HttpStatusCode.InternalServerError,
            };
            await HandleExceptionAsync(context, domainException, statusCode);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var errorResponse = new ErrorResponse
        {
            Error = new ExceptionDetails
            {
                Code = context.Response.StatusCode
            }
        };

        errorResponse.Error.Errors = exception is ValidationException validationException
            ? validationException.Errors.Select(x => new ErrorDetails { Message = $"{x.PropertyName} {x.ErrorMessage}" }).ToList()
            : new List<ErrorDetails> { new ErrorDetails { Message = exception.Message } };

        await WriteErrorResponseAsync(context, errorResponse);
    }

    private static async Task WriteErrorResponseAsync(HttpContext context, ErrorResponse error)
    {
        var httpResponse = context.Response;

        var content = JsonConvert.SerializeObject(error);

        var pipeWriter = httpResponse.BodyWriter;

        await pipeWriter.WriteAsync(Encoding.UTF8.GetBytes(content));
    }
}