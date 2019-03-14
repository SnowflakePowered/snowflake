using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Model.Game;

namespace Snowflake.Installation.Extensibility
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class SupportedPlatformAttribute : Attribute
    {
        public SupportedPlatformAttribute(string platformId)
        {
            this.PlatformId = platformId;
        }

        public PlatformId PlatformId { get; }
    }
}
