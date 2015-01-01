using System;
using Snowflake.Platform;
namespace Snowflake.Controller
{
    public interface IControllerProfileDatabase
    {
        void AddControllerProfile(IControllerProfile controllerProfile, int controllerIndex);
        IControllerProfile GetControllerProfile(string controllerId, int controllerIndex);
        string GetDeviceName(string controllerId, int controllerIndex);
        void LoadTables(System.Collections.Generic.IDictionary<string, IPlatformInfo> platforms);
        void SetDeviceName(string controllerId, int controllerIndex, string deviceName);
    }
}
