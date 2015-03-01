using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Tests.Fakes
{
    internal class FakeEmulatorAssembly : Snowflake.Emulator.IEmulatorAssembly
    {
        public Emulator.EmulatorAssemblyType AssemblyType
        {
            get { throw new NotImplementedException(); }
        }

        public string EmulatorID
        {
            get { throw new NotImplementedException(); }
        }

        public string EmulatorName
        {
            get { throw new NotImplementedException(); }
        }

        public string MainAssembly
        {
            get { throw new NotImplementedException(); }
        }
    }
}
