using CattleManager.Application.Application.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace CattleManager.Application.Application.Extensions;

public static class ExceptionMiddlewareExtensions
{
    public static void ConfigureExceptionHandler(this IApplicationBuilder app)
    {
        app.UseMiddleware<ErrorHandlerMiddleware>();
    }
}