using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;
using Snowflake.Platform;
using Snowflake.Records.Game;

namespace Snowflake.Emulator
{
    public interface IEmulatorInstance
    {
        Guid InstanceGuid { get; }
        IEmulatorAdapter EmulatorAdapter { get; }
        IList<IEmulatedPort> ControllerPorts { get; }
        int SaveSlot { get; }
        IDictionary<string, IConfigurationCollection> ConfigurationCollections { get; }
        IDictionary<string, string> InstanceMetadata { get; }
        IGameRecord Game { get; }
        IPlatformInfo Platform { get; }
        string InstancePath { get; }
        DateTime StartTime { get; }
        DateTime DestroyTime { get; }
        bool IsActive { get; }
        bool IsRunning { get; }
        bool IsGenerated { get; }
        bool IsDestroyed { get; }
        void Create();
        void Start();
        void Pause();
        void Resume();
        void Destroy();


    }
}