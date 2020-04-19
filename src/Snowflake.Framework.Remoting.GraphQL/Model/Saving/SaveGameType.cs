using HotChocolate.Types;
using Snowflake.Orchestration.Saving;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Remoting.GraphQL.Model.Saving
{
    public sealed class SaveGameType
        : ObjectType<ISaveGame>
    {
        protected override void Configure(IObjectTypeDescriptor<ISaveGame> descriptor)
        {
            descriptor.Name("SaveGame")
                .Description("Describes a single save game snapshot in a profile.");
            descriptor.Field(s => s.SaveType)
                .Description("A string value that identifies the format of the save data. All save games with the same format should use the same string.")
                .Type<NonNullType<StringType>>();
            descriptor.Field(s => s.CreatedTimestamp)
                .Description("The timestamp this save was created.")
                .Type<NonNullType<DateTimeType>>();
        }
    }
}
