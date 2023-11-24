using KioskApp.Model.ErrorHandling;

namespace KioskApp.Server.Core.Middleware
{
    public class MiddlewareConfig
    {
        private readonly RequestDelegate request;

        public MiddlewareConfig(RequestDelegate _request)
        {
            request = _request;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await request(context);
            }
            catch (InvalidOperationException e)
            {
                await HandleCustomInvalidAsync(context, e);
            }
            catch(ArgumentException e)
            {
                await HandleCustomArgumentAsync(context, e);
            }
            catch(Exception e) 
            {
                await HandleExceptionAsync(context, e);
            }
        }

        public static Task HandleExceptionAsync(HttpContext context, Exception e) 
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            return context.Response.WriteAsync(new ErrorDetails
            {
                StatusCode = context.Response.StatusCode,
                Message = e.Message
            }.ToString());
        }

        public static Task HandleCustomInvalidAsync(HttpContext context, Exception e) 
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            return context.Response.WriteAsync(new ErrorDetails
            {
                StatusCode = context.Response.StatusCode,
                Message = e.Message
            }.ToString());
        }

        public static Task HandleCustomArgumentAsync(HttpContext context, Exception e)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            return context.Response.WriteAsync(new ErrorDetails
            {
                StatusCode = context.Response.StatusCode,
                Message = e.Message
            }.ToString());
        }
    }
}
