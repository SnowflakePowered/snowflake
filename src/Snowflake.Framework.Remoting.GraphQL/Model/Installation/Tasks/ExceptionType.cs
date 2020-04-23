using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Remoting.GraphQL.Model.Installation.Tasks
{
    public sealed class ExceptionType
        : ObjectType<Exception>
    {
        protected override void Configure(IObjectTypeDescriptor<Exception> descriptor)
        {
            descriptor.Name("InstallationException")
                .Description("Describes an exception that occurred during an install task.");
            descriptor.Field("clrExceptionType")
                .Description("Gets the CLR type of the exception that occurred.")
                .Resolver(ctx => ctx.Resolver<Exception>().GetType().Name)
                .Type<NonNullType<StringType>>();
            descriptor.Field(ctx => ctx.Message)
                .Description("Gets the message of the description.")
                .Type<NonNullType<StringType>>();
        }
    }
}
