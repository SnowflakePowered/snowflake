using Snowflake.Emulator;
using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Platform;
using Snowflake.Records.File;
using Snowflake.Records.Game;
using Snowflake.Extensibility;
using Newtonsoft.Json;

namespace Snowflake.Plugin.Emulators.TestEmulator
{
    internal class TestEmulatorInstance : EmulatorInstance
    {
        private ILogger Logger { get; }
        public TestEmulatorInstance(IEmulatorAdapter emulatorAdapter,
            IGameRecord game, IFileRecord file, int saveSlot,
            IPlatformInfo platform, IList<IEmulatedPort> controllerPorts,
            ILogger logger) 
            : base(emulatorAdapter, game, file, saveSlot, platform, controllerPorts)
        {
            this.Logger = logger;
        }

        public override DateTimeOffset CreateTime { get; protected set; }
        public override DateTimeOffset StartTime { get; protected set; }
        public override DateTimeOffset DestroyTime { get; protected set; }

        public override void Create()
        {
            this.CreateTime = DateTimeOffset.UtcNow;
            this.Logger.Info(JsonConvert.SerializeObject(this.Configuration));
        }

        public override void Destroy()
        {
            this.DestroyTime = DateTimeOffset.UtcNow;
        }

        public override void Pause()
        {
            throw new NotImplementedException();
        }

        public override void Resume()
        {
            throw new NotImplementedException();
        }

        public override void Start()
        {
            this.StartTime = DateTimeOffset.UtcNow;
        }
    }
}
