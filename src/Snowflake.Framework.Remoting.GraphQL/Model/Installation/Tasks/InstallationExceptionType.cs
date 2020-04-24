using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Remoting.GraphQL.Model.Installation.Tasks
{
    public sealed class InstallationExceptionType
        : ObjectType<Exception>
    {
        protected override void Configure(IObjectTypeDescriptor<Exception> descriptor)
        {
            descriptor.Name("InstallationException")
                .Description("Describes an exception that occurred during an install task.");
            descriptor.Field("clrExceptionType")
                .Description("Gets the CLR type of the exception that occurred.")
                .Resolver(ctx =>
                {
                    var exception = ctx.Parent<Exception>();
                    if (exception is AggregateException aggregate && aggregate.InnerExceptions.Count == 1)
                    {
                        var innerException = aggregate.InnerExceptions.First();
                        return innerException.GetType().Name;
                    }
                    return exception.GetType().BaseType?.Name ?? exception.GetType().Name;
                })
                .Type<NonNullType<StringType>>();
            descriptor.Field("message")
                .Description("Gets the message of the description.")
                .Resolver(ctx =>
                {
                    var exception = ctx.Parent<Exception>();
                    if (exception is AggregateException aggregate && aggregate.InnerExceptions.Count == 1)
                    {
                        var innerException = aggregate.InnerExceptions.First();
                        return innerException.Message;
                    }
                    return exception.Message;
                })
                .Type<NonNullType<StringType>>();
        }
    }
}
