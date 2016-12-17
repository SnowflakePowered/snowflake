using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.Remoting.Framework.Exceptions
{
    public class UnknownPlatformException : RequestException
    {
        public UnknownPlatformException(string platformId) : base($"Unknown platform id {platformId}", 422)
        {
        }
    }
}
