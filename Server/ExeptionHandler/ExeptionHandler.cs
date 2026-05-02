using System.Text.Json;
namespace Implemented_MVC.ExeptionHandler;
public class ExeptionHandler
{
    private readonly RequestDelegate _next;

    public ExeptionHandler(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 500;

            var response = new
            {
                success = false,
                message = "Internal server error",
                error = ex.Message
            };

            var json = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json);
        }
    }
}