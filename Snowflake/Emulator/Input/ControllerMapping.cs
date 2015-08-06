using System.Collections.Generic;
using Snowflake.Extensions;

namespace Snowflake.Emulator.Input
{
    public class ControllerMapping : IControllerMapping
    {
        public ControllerMappingType MappingType { get; }
        public IDictionary<string, string> KeyMappings { get; }
        public IReadOnlyDictionary<string, string> InputMappings => this.inputMappings.AsReadOnly();
        private readonly IDictionary<string, string> inputMappings;

        public ControllerMapping(ControllerMappingType mappingType, IDictionary<string, string> keyMappings, IDictionary<string, string> inputMappings)
        {
            this.MappingType = mappingType;
            this.KeyMappings = keyMappings;
            this.inputMappings = inputMappings;
        }
    }
}
