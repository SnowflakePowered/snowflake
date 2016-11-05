using System;
using System.Collections.Generic;

namespace Snowflake.Configuration
{
    public interface IConfigurationCollectionDescriptor 
    {
        IDictionary<string, string> Outputs { get; }
        string GetDestination(string sectionKey);
    }
}