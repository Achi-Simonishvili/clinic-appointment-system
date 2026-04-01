using ClinicSystem.Application.Common;
using ClinicSystem.Application.Common.Exceptions;
using System.Net;

namespace ClinicSystem.API.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext ctx)
    {
        try
        {
            await _next(ctx);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(ctx, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext ctx, Exception ex)
    {
        var response = new CommonResponse
        {
            IsSuccess = false,
            Message = ex.Message
        };

        response.StatusCode = ex switch
        {
            NotFoundException => HttpStatusCode.NotFound,
            BadRequestException => HttpStatusCode.BadRequest,
            ForbidException => HttpStatusCode.Forbidden,
            ArgumentException => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError
        };

        ctx.Response.ContentType = "application/json";
        ctx.Response.StatusCode = (int)response.StatusCode;
        return ctx.Response.WriteAsJsonAsync(response);
    }
}