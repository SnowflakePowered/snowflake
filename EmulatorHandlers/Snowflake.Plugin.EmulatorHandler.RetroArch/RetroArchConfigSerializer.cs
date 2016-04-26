using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;

namespace Snowflake.Plugin.EmulatorHandler.RetroArch.Serializer
{
    public class RetroArchConfigSerializer : KeyValuePairConfigurationSerializer
    {

        public RetroArchConfigSerializer() : base(BooleanMapping.LowercaseBooleanMapping, "nul", "=")
        {
        }

    }
}
