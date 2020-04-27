using HotChocolate.Types;
using Snowflake.Remoting.GraphQL.FrameworkQueries.Mutations.Relay;
using Snowflake.Remoting.GraphQL.Model.Filesystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Installation
{
    internal sealed class CreateInstallationInput
        : RelayMutationBase
    {
        public Guid GameID { get; set; }
        public List<FileSystemInfo> Artifacts { get; set; }
        public string Installer { get; set; }
    }

    internal sealed class CreateInstallationInputType
        : InputObjectType<CreateInstallationInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateInstallationInput> descriptor)
        {
            descriptor.Name(nameof(CreateInstallationInput))
                .WithClientMutationId();

            descriptor.Field(i => i.GameID)
                .Name("gameId")
                .Description("The `gameId` GUID of the game to install to.")
                .Type<NonNullType<UuidType>>();

            descriptor.Field(i => i.Artifacts)
                .Description("The artifacts (collection of files and folders) to install. This should be retrieved by the `filesystem.installables.artifacts` Query field.")
                .Type<NonNullType<ListType<NonNullType<OSTaggedFileSystemPathType>>>>();

            descriptor.Field(i => i.Installer)
                .Description("The installer that will install the specified artifacts. This should be retrieved by the `filesystem.installables.installer` Query field.")
                .Type<NonNullType<ListType<NonNullType<StringType>>>>();
        }
    }
}
