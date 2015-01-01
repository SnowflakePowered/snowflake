using System;
using System.Collections.Generic;
namespace Snowflake.Emulator.Input
{
    public interface IControllerMapping
    {
        IReadOnlyDictionary<string, string> InputMappings { get; }
        IDictionary<string, string> KeyMappings { get; }
        ControllerMappingType MappingType { get; }
    }
}
