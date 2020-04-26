using HotChocolate.Types;
using Snowflake.Orchestration.Extensibility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Remoting.GraphQL.Model.Orchestration
{
    public sealed class GameEmulationStateEnum
        : EnumType<GameEmulationState>
    {
        protected override void Configure(IEnumTypeDescriptor<GameEmulationState> descriptor)
        {
            descriptor.Name("GameEmulationState")
                .Description("Describes the state of a game emulation.");
            descriptor.Value(GameEmulationState.RequiresSetupEnvironment)
                .Description(
@"This game emulation requires environment setup. The following mutations may occur.

* `setupEmulationEnvironment`
* `cleanupEmulation`
");
            descriptor.Value(GameEmulationState.RequiresCompileConfiguration)
                .Description(
@"The game emulation requires configuration to be compiled. The following mutations may occur.

* `compileEmulationConfiguration`
* `cleanupEmulation`
");
            descriptor.Value(GameEmulationState.RequiresRestoreSaveGame)
                .Description(
@"The game emulation requires the save game to be restored. The following mutations may occur.

* `restoreEmulationSave`
* `cleanupEmulation`
");
            descriptor.Value(GameEmulationState.CanStartEmulation)
                            .Description(
@"The game emulation can be started. The following mutations may occur.

* `persistEmulationSave`
* `startEmulation`
* `cleanupEmulation`
");
            descriptor.Value(GameEmulationState.CanStopEmulation)
                            .Description(
@"The game emulation is running, and can be stopped. The following mutations may occur.

* `persistEmulationSave`
* `stopEmulation`
* `cleanupEmulation` will immediately stop the emulation and cleanup.
");
            descriptor.Value(GameEmulationState.RequiresDispose)
                           .Description(
@"The game emulation has been stopped, and needs cleanup. The following mutations may occur.

* `persistEmulationSave`
* `cleanupEmulation`
");
        }
    }
}
