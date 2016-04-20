using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;

namespace Snowflake.Plugin.EmulatorHandler.RetroArch
{
    class RetroArchConfigSerializer : IConfigurationSerializer
    {
        public IConfigurationTypeMapper TypeMapper
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string Serialize(IIterableConfigurationSection iterableConfigurationSection)
        {
            throw new NotImplementedException();
        }

        public string Serialize(IConfigurationSection configurationSection)
        {
            throw new NotImplementedException();
        }

        public string SerializeIterableLine<T>(string key, T value, int iteration)
        {
            throw new NotImplementedException();
        }

        public string SerializeLine<T>(string key, T value)
        {
            throw new NotImplementedException();
        }

        public string SerializeValue(object value)
        {
            throw new NotImplementedException();
        }
    }
}
