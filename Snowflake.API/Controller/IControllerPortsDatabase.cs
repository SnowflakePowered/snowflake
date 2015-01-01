using System;
using Snowflake.Platform;
namespace Snowflake.Controller
{
    public interface IControllerPortsDatabase
    {
        void AddPlatform(IPlatformInfo platformInfo);
        string GetPort(IPlatformInfo platformInfo, int portNumber);
        void SetPort(IPlatformInfo platformInfo, int portNumber, string controllerId);
    }
}
