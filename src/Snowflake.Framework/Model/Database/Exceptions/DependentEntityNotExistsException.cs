using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Model.Database.Exceptions
{
    public class DependentEntityNotExistsException : Exception
    {
        public DependentEntityNotExistsException(Guid entityGuid)
        {
        }
    }
}
