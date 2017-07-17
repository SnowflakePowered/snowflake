using Snowflake.Remoting.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.Remoting.Framework.Exceptions
{
    public class UnknownStoneException : RequestException
    {
        public UnknownStoneException(string stoneId) : base($"Unknown Stone Element {stoneId}", 422)
        {
        }
    }
}
