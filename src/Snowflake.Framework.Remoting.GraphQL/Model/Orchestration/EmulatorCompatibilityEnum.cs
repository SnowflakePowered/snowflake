using HotChocolate.Types;
using Snowflake.Orchestration.Extensibility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Remoting.GraphQL.Model.Orchestration
{
    public sealed class EmulatorCompatibilityEnum
        : EnumType<EmulatorCompatibility>
    {
        protected override void Configure(IEnumTypeDescriptor<EmulatorCompatibility> descriptor)
        {
            descriptor.Name("EmulatorCompatibility")
                .Description("Describes the levels of compatibility an emulator has with a given game.");
            descriptor.Value(EmulatorCompatibility.Unsupported)
                .Description("This game is not supported by this emulator.");
            descriptor.Value(EmulatorCompatibility.MissingSystemFiles)
                .Description("The system files required to run this game are not available.");
            descriptor.Value(EmulatorCompatibility.RequiresValidation)
                .Description("The game has the files required to be run, but they are not in the correct format. " +
                "Validation should be run before launching this game.");
            descriptor.Value(EmulatorCompatibility.Ready)
                .Description("This game is supported and ready to be run, requiring no validation. ");
        }
    }
}
