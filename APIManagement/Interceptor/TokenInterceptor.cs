using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace APIManagement.Interceptor
{
    public class TokenInterceptor
    {
        private readonly RequestDelegate _next;
        private readonly IStorage _storage;
        public TokenInterceptor(RequestDelegate next, IStorage storage)
        {
            this._next = next;
            this._storage = storage;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Headers.ContainsKey("x-client-secret"))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Client secret not found in header");
                return;
            }
            else if (context.Request.Headers.ContainsKey("x-client-secret"))
            {
                string token = context.Request.Headers["x-client-secret"];
                if (string.IsNullOrEmpty(token) || !this._storage.ValidateSecret(token))
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("Invalid client secret");
                    return;
                }
            }

            await this._next.Invoke(context).ConfigureAwait(false);

        }
    }
    public static class TokenExtension
    {
        public static IApplicationBuilder UseToken(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TokenInterceptor>();
        }
    }
}
