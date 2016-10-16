using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.DynamicConfiguration
{
    public interface IConfigurationOption
    {
        string DisplayName { get; }
        string Description { get; }
        bool Simple { get; }
        bool Private { get; }
        bool Flag { get; }
        double Max { get; }
        double Min { get; }
        double Increment { get; }
        bool IsPath { get; }
        string OptionName { get; }
        string KeyName { get; }
        object Default { get; }
        Type Type { get; }
        IDictionary<string, object> CustomMetadata { get; }
    }

}
