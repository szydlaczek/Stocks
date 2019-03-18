using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
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
            var ip_date = string.Concat(ip, "-", "Date");

            if (Cache.TryGetValue(blockedKey, out bool entry))
            {
                var date = Cache.Get(ip_date);
                c.HttpContext.Response.Headers.Add("Blocked-to", date.ToString());
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
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(SecondsToBlock));

                    Cache.Set(blockedKey, true, cacheEntryOptions);
                    c.Result = new BadRequestResult();
                    var date = DateTime.Now.AddSeconds(SecondsToBlock);
                    Cache.Set(ip_date, date, cacheEntryOptions);
                    c.HttpContext.Response.Headers.Add("Blocked-to", date.ToString());
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