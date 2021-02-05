using HotChocolate.Types;
using Snowflake.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Remoting.GraphQL.Model.Configuration
{
    public sealed class ConfigurationCollectionType
        : ObjectType<IConfigurationCollection>
    {
        protected override void Configure(IObjectTypeDescriptor<IConfigurationCollection> descriptor)
        {
            descriptor.Name("ConfigurationCollection")
                .Description("Describes a collection of configuration sections which form the set of configurations needed to execute a task.");
            descriptor.Field("sectionDescriptors")
                .Description("Describes the sections contained within the configuration collection.")
                .Resolve(ctx => ctx.Parent<IConfigurationCollection>().Select(c => c.Value.Descriptor))
                .Type<NonNullType<ListType<NonNullType<SectionDescriptorType>>>>();
            descriptor.Field("collectionId")
                .Description("The GUID that refers to this specific collection of values.")
                .Type<UuidType>()
                .Resolve(ctx => ctx.Parent<IConfigurationCollection>().ValueCollection.Guid);
            descriptor.Field("values")
                .Description("The list of values that make up this collection.")
                .Argument("sectionKey", arg => arg.Description("The configuration section for which to return values.").Type<StringType>())
                .Argument("valueId", arg => arg.Description("Return a specific value with the given GUID. This overrides `sectionKey` if set.").Type<UuidType>())
                .Resolve(ctx =>
                {
                    var collection = ctx.Parent<IConfigurationCollection>();
                    var valueCollection = collection.ValueCollection;
                    string sectionKey = ctx.ArgumentValue<string>("sectionKey");
                    Guid valueID = ctx.ArgumentValue<Guid>("valueId");

                    if (valueID != default)
                    {
                        var valueTuple = valueCollection[valueID];
                        if (valueTuple.value == null) return Enumerable.Empty<(string, string, IConfigurationValue)>();
                        return new[] { valueTuple };
                    }
                    if (sectionKey != null)
                    {
                        return collection[sectionKey]?
                         .Values.Select(kvp => (sectionKey, kvp.Key, kvp.Value))
                         ?? Enumerable.Empty<(string, string, IConfigurationValue)>();
                    }
                    return valueCollection;
                })
                .Type<NonNullType<ListType<NonNullType<NamedConfigurationValueType>>>>();
        }
    }
}
