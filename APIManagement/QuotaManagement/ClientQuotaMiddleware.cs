using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIManagement
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ClientQuotaMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IStorage _storage;

        public ClientQuotaMiddleware(RequestDelegate next, IStorage storage)
        {
            _next = next;
            _storage = storage;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            string clientHeader = "x-client-secret";
            string clientId = "";
            if (httpContext.Request.Headers.Keys.Contains(clientHeader))
            {
                clientId = httpContext.Request.Headers[clientHeader].First();
            }
            Client clientObject = _storage.GetClientById(clientId);
            int reqLimit = GetLimit(clientObject);
            // new counter
            QuotaCounter counter = new QuotaCounter(DateTime.UtcNow, 1);

            var entry = _storage.GetCounterByClientId(clientId);
            // timer not expired
            if (entry != null && entry.Timestamp + TimeSpan.FromSeconds(30) >= DateTime.UtcNow)
            {
                var totalRequests = entry.TotalRequests + 1;
                // deep copy
                counter = new QuotaCounter(entry.Timestamp, totalRequests);
            }

            // timer expired
            if(entry != null &&  entry.Timestamp + TimeSpan.FromSeconds(30) < DateTime.UtcNow )
            {
                _storage.RemoveCounter(clientId);
            }

            if (counter.TotalRequests > reqLimit)
            {                
                httpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await httpContext.Response.WriteAsync($"Daily quota for service tier {clientObject.ServiceTier}: {reqLimit}  exceeded. Quota will reset at {entry.Timestamp + TimeSpan.FromSeconds(30)}");
                return;
            }
            else
            {
                _storage.SetCounter(clientId, counter);
            }
            await this._next.Invoke(httpContext).ConfigureAwait(false);
        }

        private static int GetLimit(Client client)
        {
            if (client.ServiceTier == ServiceTier.Basic.ToString())
                return 1;
            if (client.ServiceTier == ServiceTier.Standard.ToString())
                return 10;
            return 0;
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ClientQuotaMiddlewareExtensions
    {
        public static IApplicationBuilder UseClientQuotaMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ClientQuotaMiddleware>();
        }
    }
}
