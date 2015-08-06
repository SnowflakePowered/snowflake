using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Extensions;
namespace Snowflake.Emulator.Input
{
    public class ControllerMapping : IControllerMapping
    {
        public ControllerMappingType MappingType { get; }
        public IDictionary<string, string> KeyMappings { get; }
        public IReadOnlyDictionary<string, string> InputMappings { get { return this.inputMappings.AsReadOnly(); } }
        private IDictionary<string, string> inputMappings;

        public ControllerMapping(ControllerMappingType mappingType, IDictionary<string, string> keyMappings, IDictionary<string, string> inputMappings)
        {
            this.MappingType = mappingType;
            this.KeyMappings = keyMappings;
            this.inputMappings = inputMappings;
        }
    }
}
