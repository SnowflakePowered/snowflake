using System;

namespace Snowflake.Configuration
{
    public interface IConfigurationValue
    {
        object Value { get; set; }
        IConfigurationOption Option { get; }
        Guid Guid { get; }
    }
}