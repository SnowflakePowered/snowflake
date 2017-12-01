using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Configuration
{
    public interface ISelectionOptionDescriptor
    {
        string DisplayName { get; }
        bool Private { get; }
        string SerializeAs { get; }
        int NumericValue { get; }
    }
}
