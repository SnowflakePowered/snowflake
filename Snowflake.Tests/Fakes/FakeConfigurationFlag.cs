using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Tests.Fakes
{
    internal class FakeConfigurationFlag : Snowflake.Emulator.Configuration.IConfigurationFlag
    {
        public string DefaultValue
        {
            get { throw new NotImplementedException(); }
        }

        public string Description
        {
            get { throw new NotImplementedException(); }
        }

        public string Key
        {
            get { throw new NotImplementedException(); }
        }

        public IList<Emulator.Configuration.IConfigurationFlagSelectValue> SelectValues
        {
            get { throw new NotImplementedException(); }
        }

        public int RangeMin
        {
            get { throw new NotImplementedException(); }
        }

        public int RangeMax
        {
            get { throw new NotImplementedException(); }
        }

        public Emulator.Configuration.ConfigurationFlagTypes Type
        {
            get { throw new NotImplementedException(); }
        }
    }
}
