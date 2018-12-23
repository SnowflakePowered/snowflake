using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Configuration
{
    public interface IConfigurationValueCollection 
        : IEnumerable<(string section, string option, IConfigurationValue value)>
    {
        Guid ValueCollectionGuid { get; }

        IConfigurationValue? this[IConfigurationSectionDescriptor descriptor, string option] { get; }
        IReadOnlyDictionary<string, IConfigurationValue> this[IConfigurationSectionDescriptor descriptor] { get; }
    }
}
