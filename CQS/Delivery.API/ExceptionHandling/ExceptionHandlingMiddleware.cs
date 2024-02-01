using FluentValidation;

namespace Delivery.API.ExceptionHandling;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException e)
        {
            var response = new ErrorResponse
            {
                Descriptions = e.Errors.Select(x => x.ErrorMessage).ToArray()
            };

            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}

public sealed class ErrorResponse
{
    public string[] Descriptions { get; set; }
}