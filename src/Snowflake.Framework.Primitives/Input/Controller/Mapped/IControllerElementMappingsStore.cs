using System.Collections.Generic;
using Snowflake.Input.Controller.Mapped;

namespace Snowflake.Input.Controller.Mapped
{
    public interface IControllerElementMappingsStore
    {
        void AddMappings(IControllerElementMappings mappings, string profileName);
        void DeleteMappings(string controllerId, string deviceId);
        void DeleteMappings(string controllerId, string deviceId, string profileName);
        IEnumerable<IControllerElementMappings> GetMappings(string controllerId, string deviceId);
        IControllerElementMappings GetMappings(string controllerId, string deviceId, string profileName);
        void UpdateMappings(IControllerElementMappings mappings, string profileName);
    }
}
