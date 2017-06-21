using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Extensibility
{
    [System.AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class ImportServiceAttribute : Attribute
    {
      
        // This is a positional argument
        public ImportServiceAttribute(Type service)
        {
            this.Service = service;
        }

        public Type Service { get; }
    }
}
