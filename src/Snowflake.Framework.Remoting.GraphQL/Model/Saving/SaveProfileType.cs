using HotChocolate.Types;
using Snowflake.Orchestration.Saving;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Remoting.GraphQL.Model.Saving
{
    public sealed class SaveProfileType
        : ObjectType<ISaveProfile>
    {
        protected override void Configure(IObjectTypeDescriptor<ISaveProfile> descriptor)
        {
            descriptor.Name("SaveProfile")
                .Description("Describes a save profile, which is a linked list of save games, forming a lineage of saves from the first game in the profile " +
                "to the head");
            descriptor.Field(s => s.Guid)
                .Name("profileId")
                .Description("The GUID of the save profile. This GUID is contextualized, and only has meaning when the Game this save belongs to is known.")
                .Type<NonNullType<UuidType>>();
            descriptor.Field(s => s.ManagementStrategy)
                .Description("The strategy used to manage save games in this profile.")
                .Type<SaveManagementStrategyEnum>();
            descriptor.Field(s => s.SaveType)
                .Description("A string value that identifies the format of the save data. All save games with the same format should use the same string.")
                .Type<NonNullType<StringType>>();
            descriptor.Field("head")
                .Description("The 'head' save, or the latest save in the profile. If the profile is empty, this is null.")
                .Resolve(ctx => ctx.Parent<ISaveProfile>().GetHeadSave())
                .Type<SaveGameType>();
            descriptor.Field("history")
                .Description("All saves in the profile history. " +
                "If not using a DIFF or COPY strategy, this will consist only of the head save. " +
                "If this profile is empty, consists of an empty array.")
                .Resolve(ctx => ctx.Parent<ISaveProfile>().GetHistory())
                .Type<NonNullType<ListType<NonNullType<SaveGameType>>>>();
        }
    }
}
