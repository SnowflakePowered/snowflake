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
                .Type<NonNullType<UuidType>>();

            descriptor.Field(i => i.Artifacts)
                .Type<NonNullType<ListType<NonNullType<OSTaggedFileSystemPathType>>>>();

            descriptor.Field(i => i.Installer)
                .Type<NonNullType<ListType<NonNullType<StringType>>>>();
        }
    }
}
