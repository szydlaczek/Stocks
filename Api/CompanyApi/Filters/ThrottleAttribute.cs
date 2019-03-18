using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Net;

namespace CompanyApi.Filters
{
    public class ThrottleAttribute : ActionFilterAttribute
    {
        public string Name { get; set; }

        public int SecondsToBlock { get; set; }
        public int RequestLimit { get; set; }
        public int RequestLimitTime { get; set; }

        public string Message { get; set; }
        private static MemoryCache Cache { get; } = new MemoryCache(new MemoryCacheOptions());

        public override void OnActionExecuting(ActionExecutingContext c)
        {
            HttpStatusCode code = HttpStatusCode.OK;
            var ip = c.HttpContext.Request.HttpContext.Connection.RemoteIpAddress;
            var blockedKey = string.Concat("blocked", "-", ip);

            if (Cache.TryGetValue(blockedKey, out bool entry))
            {
                code = HttpStatusCode.InternalServerError;
                c.Result = new BadRequestResult();
                c.HttpContext.Response.StatusCode = (int)code;
            }
            else
            if (Cache.TryGetValue(ip, out List<DateTime> entries))
            {
                if (entries.Count < RequestLimit)
                {
                    entries.Add(DateTime.Now);
                }
                else
                {
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(60));
                    Cache.Set(blockedKey, true, cacheEntryOptions);
                    c.Result = new BadRequestResult();
                }
            }
            else
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(RequestLimitTime));
                Cache.Set(ip, new List<DateTime> { DateTime.Now }, cacheEntryOptions);
            }
        }
    }
}