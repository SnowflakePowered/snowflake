using HotChocolate.Language;
using HotChocolate.Types;
using Snowflake.Extensibility.Queueing;
using Snowflake.Filesystem;
using Snowflake.Installation;
using Snowflake.Remoting.GraphQL;
using Snowflake.Scraping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Queries.Queueing
{
    public sealed class JobQueries
        : ObjectTypeExtension
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Name("JobQuery");
            descriptor.Field("scraping")
                .Resolve(ctx =>
                {
                    return ctx.SnowflakeService<IAsyncJobQueueFactory>()
                        .GetJobQueue<IScrapeContext, IEnumerable<ISeed>>(false);
                })
                .Type<NonNullType<ScrapingJobQueueType>>();
            descriptor.Field("installation")
                .Resolve(ctx =>
                {
                    return ctx.SnowflakeService<IAsyncJobQueueFactory>()
                        .GetJobQueue<TaskResult<IFile>>();
                })
                .Type<NonNullType<InstallationJobQueueType>>();
        }
    }
}
