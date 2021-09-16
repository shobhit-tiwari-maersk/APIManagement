using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace APIManagement
{
    public class IpAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<IpAuthMiddleware> _logger;
        private readonly byte[][] _safelist;

        public IpAuthMiddleware(
            RequestDelegate next,
            ILogger<IpAuthMiddleware> logger,
            string safelist)
        {
            var ips = safelist.Split(';');
            _safelist = new byte[ips.Length][];
            for (var i = 0; i < ips.Length; i++)
            {
                _safelist[i] = IPAddress.Parse(ips[i]).GetAddressBytes();
            }

            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            //if (context.Request.Method != HttpMethod.Get.Method)
            //{
                var remoteIp = context.Connection.RemoteIpAddress;
                _logger.LogDebug("Request from Remote IP address: {RemoteIp}", remoteIp);

                var bytes = remoteIp.GetAddressBytes();
                var badIp = true;
                foreach (var address in _safelist)
                {
                    if (address.SequenceEqual(bytes))
                    {
                        badIp = false;
                        break;
                    }
                }

                if (badIp)
                {
                    _logger.LogWarning(
                        "Forbidden Request from Remote IP address: {RemoteIp}", remoteIp);
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                await context.Response.WriteAsync($"Source Ip : {remoteIp} does not have access");
                    return;
                }
           // }

            await _next.Invoke(context);
        }
    }
}
