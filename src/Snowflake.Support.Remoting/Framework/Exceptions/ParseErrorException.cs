using Snowflake.Remoting.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Framework.Exceptions
{
    public class ParseErrorException : RequestException
    {
        public ParseErrorException(string from, Type convertingType) 
            : base($"Error when attempting to parse a {convertingType.Name} from data {from}", 400)
        {
        }
    }
}
