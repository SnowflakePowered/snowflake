using System;

namespace Snowflake.DynamicConfiguration
{
    public interface IConfigurationValue
    {
        object Value { get; set; }
        IConfigurationOption Option { get; }
        Guid Guid { get; }
    }
}