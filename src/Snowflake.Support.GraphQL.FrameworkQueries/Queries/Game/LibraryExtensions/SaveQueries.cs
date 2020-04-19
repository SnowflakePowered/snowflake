using HotChocolate;
using HotChocolate.Types;
using Snowflake.Remoting.GraphQL.Model.Saving;
using Snowflake.Model.Game;
using Snowflake.Model.Game.LibraryExtensions;
using Snowflake.Orchestration.Saving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Support.GraphQLFrameworkQueries.Queries.Game
{
    public sealed class SaveQueries
        : ObjectTypeExtension<IGame>
    {
        protected override void Configure(IObjectTypeDescriptor<IGame> descriptor)
        {
            descriptor.Name("Game");

            descriptor.Field("saves")
                .Argument("saveType", arg => 
                    arg.Description("A string that identifies all save profiles with the same format.")
                    .Type<StringType>())
                .Argument("profileId", arg =>
                    arg.Description("The GUID of a specific profile.")
                    .Type<UuidType>())
                .Description("The save profiles associated with this game.")
                .Resolver(ctx =>
                {
                    var profileId = ctx.Argument<Guid>("profileId");
                    var saveType = ctx.Argument<string>("saveType");
                    var saves = ctx.Parent<IGame>().WithFiles().WithSaves();
                    if (profileId != default)
                    {
                        var profile = saves.GetProfile(profileId);

                        // Check for save type, to maintain consistent behaviour if both arguments are provided.
                        if (saveType != null) return profile?.SaveType == saveType ?
                            new[] { profile } : Enumerable.Empty<ISaveProfile>();

                        // saveType has no value.
                        return profile != null
                            ? new[] { profile }
                            : Enumerable.Empty<ISaveProfile>();
                    }

                    if (saveType != null) 
                        return saves.GetProfiles(saveType);

                    return saves.GetProfiles();
                })
                .Type<NonNullType<ListType<SaveProfileType>>>();
        }
    }
}
