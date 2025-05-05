using System.Text.Json;
using TasksManagement.Infrastructure.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;

namespace TasksManagement.Infrastructure.Middleware
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            int statusCode;
            object errorResponse;

            switch (exception)
            {
                case NotFoundException notFoundEx:
                    statusCode = StatusCodes.Status404NotFound;
                    errorResponse = new
                    {
                        Status = statusCode,
                        Message = notFoundEx.Message
                    };
                    break;

                case AlreadyExistsException alreadyExistsEx:
                    statusCode = StatusCodes.Status409Conflict;
                    errorResponse = new
                    {
                        Status = statusCode,
                        Message = alreadyExistsEx.Message
                    };
                    break;

                case ValidationException validationEx:
                    statusCode = StatusCodes.Status400BadRequest;
                    errorResponse = new
                    {
                        Status = statusCode,
                        Message = "Validation failed.",
                        Errors = validationEx.Errors.Select(e => new
                        {
                            Property = e.PropertyName,
                            Error = e.ErrorMessage
                        })
                    };
                    break;

                default:
                    statusCode = StatusCodes.Status500InternalServerError;
                    errorResponse = new
                    {
                        Status = statusCode,
                        Message = "An unexpected error occurred."
                    };
                    break;
            }

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var json = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(json);
        }
    }

    public static class GlobalExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalExceptionHandlerMiddleware>();
        }
    }

}