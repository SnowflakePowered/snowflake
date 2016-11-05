using System;

namespace Snowflake.Configuration
{
    public interface IConfigurationValue
    {
        object Value { get; set; }
        Guid Guid { get; }
    }
}