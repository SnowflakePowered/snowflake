using Mono.Net;

namespace Snowflake.Extensions
{
    public static class HttpListenerContextExts
    {
        public static void AddAccessControlHeaders(this HttpListenerContext context)
        {
            context.Response.AppendHeader("Access-Control-Allow-Credentials", "true");
            context.Response.AppendHeader("Access-Control-Allow-Origin", "*");
            context.Response.AppendHeader("Access-Control-Origin", "*");
            context.Response.AppendHeader("Access-Control-Allow-Headers", "*");
            context.Response.AppendHeader("Allow-Control-Allow-Origin", "*");
        }
    }
}
