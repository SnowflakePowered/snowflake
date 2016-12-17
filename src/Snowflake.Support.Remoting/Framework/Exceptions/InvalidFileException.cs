using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.Remoting.Framework.Exceptions
{
    public class InvalidFileException : RequestException
    {
        public InvalidFileException() : base("Invalid path or mimetype", 400) { }
    }
}
