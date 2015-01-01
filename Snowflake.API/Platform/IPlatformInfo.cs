using System;
using System.Collections.Generic;
using Snowflake.Controller;
using Snowflake.Information;
namespace Snowflake.Platform
{
    public interface IPlatformInfo : IInfo
    {
        IReadOnlyDictionary<string, IControllerDefinition> Controllers { get; }
        IPlatformDefaults Defaults { get; set; }
        IList<string> FileExtensions { get; }
        int MaximumInputs { get; }
    }
}
