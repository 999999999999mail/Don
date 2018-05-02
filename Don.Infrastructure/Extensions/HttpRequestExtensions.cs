using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Linq;

namespace Don.Infrastructure.Extensions
{
    public static class HttpRequestExtensions
    {
        public static string GetClientIP(this HttpRequest httpRequest, bool tryUseXForwardHeader = true)
        {
            // todo support new "Forwarded" header (2014) https://en.wikipedia.org/wiki/X-Forwarded-For

            // X-Forwarded-For (csv list):  Using the First entry in the list seems to work
            // for 99% of cases however it has been suggested that a better (although tedious)
            // approach might be to read each IP from right to left and use the first public IP.
            // http://stackoverflow.com/a/43554000/538763
            //
            string ip = null;
            if (tryUseXForwardHeader)
                ip = httpRequest.GetHeaderValue("X-Forwarded-For").SplitCsv().FirstOrDefault();

            // RemoteIpAddress is always null in DNX RC1 Update1 (bug).
            if (ip.IsNullOrWhitespace())
                ip = httpRequest.HttpContext?.Connection?.RemoteIpAddress?.ToString();

            if (ip.IsNullOrWhitespace())
                ip = httpRequest.GetHeaderValue("REMOTE_ADDR");

            return (ip ?? "");
        }

        public static string GetReferer(this HttpRequest httpRequest)
        {
            var referer = httpRequest.GetHeaderValue("Referer");
            return referer;
        }


        public static string GetHeaderValue(this HttpRequest httpRequest, string headerName)
        {
            if (httpRequest?.Headers?.TryGetValue(headerName, out StringValues values) ?? false)
            {
                return values.ToString();
            }
            return "";
        }
    }
}
