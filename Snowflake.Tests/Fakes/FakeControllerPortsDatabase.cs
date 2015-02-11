using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Tests.Fakes
{
    internal class FakeControllerPortsDatabase : Snowflake.Controller.IControllerPortsDatabase

    {
        public void AddPlatform(Platform.IPlatformInfo platformInfo)
        {
            throw new NotImplementedException();
        }

        public string GetDeviceInPort(Platform.IPlatformInfo platformInfo, int portNumber)
        {
            throw new NotImplementedException();
        }

        public void SetDeviceInPort(Platform.IPlatformInfo platformInfo, int portNumber, string controllerId)
        {
            throw new NotImplementedException();
        }
    }
}
