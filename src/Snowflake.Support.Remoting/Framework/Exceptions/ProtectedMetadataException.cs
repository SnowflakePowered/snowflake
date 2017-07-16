using Snowflake.Remoting.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.Remoting.Framework.Exceptions
{
    public class ProtectedMetadataException : RequestException
    {
        internal ProtectedMetadataException(string metadataKey) 
            : base($"The metadata key {metadataKey} is protected and can not be removed", 405)
        {
        }
    }
}
