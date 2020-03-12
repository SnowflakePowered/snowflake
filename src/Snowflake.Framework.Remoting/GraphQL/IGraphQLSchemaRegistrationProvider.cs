using HotChocolate;
using Snowflake.Loader;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Framework.Remoting.GraphQL
{
    public interface IGraphQLSchemaRegistrationProvider
    {
        void RegisterSchema(IServiceRepository loaderServices, string schemaNamespace, string schemaName, Action<ISchemaBuilder> schemaBuilder);
    }
}
