using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Core.API;
using Newtonsoft.Json;
namespace Snowflake.Core.API.JSAPI
{
    public static class JSBridge
    {
        public static string GetAllPlatforms(JSRequest request)
        {
            return JsonConvert.SerializeObject(CoreAPI.GetAllPlatforms());
        }

        public static string Work(JSRequest request)
        {
            System.Threading.Thread.Sleep(100000);
            return "done";
        }
    }
}
