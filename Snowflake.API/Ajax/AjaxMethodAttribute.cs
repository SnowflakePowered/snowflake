using System;
using System.Security.Policy;

namespace Snowflake.Ajax
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class AjaxMethodAttribute : Attribute
    {
        public string MethodName { get; set; }
        public string MethodPrefix { get; set; }
      
    }
}