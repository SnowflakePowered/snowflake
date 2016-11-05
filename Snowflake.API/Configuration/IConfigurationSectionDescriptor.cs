using System;
using System.Collections.Generic;

namespace Snowflake.Configuration
{
    public interface IConfigurationSectionDescriptor 
    {
        string Description { get; }
        string DisplayName { get; }
        string SectionName { get; }
        IDictionary<string, IConfigurationOption> Options { get; }
    }
}