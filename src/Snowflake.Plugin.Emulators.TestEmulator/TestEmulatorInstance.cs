using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Snowflake.Emulator;
using Snowflake.Extensibility;
using Snowflake.Platform;
using Snowflake.Records.File;
using Snowflake.Records.Game;

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

        /// <inheritdoc/>
        public override DateTimeOffset CreateTime { get; protected set; }

        /// <inheritdoc/>
        public override DateTimeOffset StartTime { get; protected set; }

        /// <inheritdoc/>
        public override DateTimeOffset DestroyTime { get; protected set; }

        /// <inheritdoc/>
        public override void Create()
        {
            this.CreateTime = DateTimeOffset.UtcNow;
            this.Logger.Info(JsonConvert.SerializeObject(this.Configuration));
        }

        /// <inheritdoc/>
        public override void Destroy()
        {
            this.DestroyTime = DateTimeOffset.UtcNow;
        }

        /// <inheritdoc/>
        public override void Pause()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void Resume()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void Start()
        {
            this.StartTime = DateTimeOffset.UtcNow;
        }
    }
}
