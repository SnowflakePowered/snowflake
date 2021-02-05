﻿using HotChocolate.Types;
using Snowflake.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Remoting.GraphQL.Model.Configuration
{
    public sealed class ConfigurationSectionType
        : ObjectType<IConfigurationSection>
    {
        protected override void Configure(IObjectTypeDescriptor<IConfigurationSection> descriptor)
        {
            descriptor.Name("ConfigurationSection")
                .Description("Describes a single, standalone configuration section that does not belong to a configuration collection.");
            descriptor.Field("sectionDescriptor")
                .Description("Describes this configuration section.")
                .Resolve(ctx => ctx.Parent<IConfigurationSection>().Descriptor)
                .Type<NonNullType<SectionDescriptorType>>();
            descriptor.Field("collectionId")
                .Description("The GUID that refers to this specific collection of values.")
                .Type<UuidType>()
                .Resolve(ctx => ctx.Parent<IConfigurationSection>().ValueCollection.Guid);
            descriptor.Field("values")
                .Description("The list of values that make up this collection.")
                .Argument("valueId", arg => arg.Description("Return a specific value with the given GUID.").Type<UuidType>())
                .Resolve(ctx => {
                    var section = ctx.Parent<IConfigurationSection>();
                    Guid valueID = ctx.ArgumentValue<Guid>("valueID");

                    if (valueID != default)
                    {
                        var valueTuple = section.ValueCollection[valueID];
                        if (valueTuple.value == null) return Enumerable.Empty<(string, string, IConfigurationValue)>();
                        return new[] { valueTuple };
                    }
                    return section.Values.Select(kvp => (section.Descriptor.SectionKey, kvp.Key, kvp.Value))
                        ?? Enumerable.Empty<(string, string, IConfigurationValue)>();
                })
                .Type<NonNullType<ListType<NonNullType<NamedConfigurationValueType>>>>();
        }
    }
}
