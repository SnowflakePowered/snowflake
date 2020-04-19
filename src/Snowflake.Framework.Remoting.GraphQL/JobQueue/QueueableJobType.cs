using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Remoting.GraphQL.JobQueue
{
    public sealed class QueueableJobType
        : ObjectType<(Guid queueableJob, object asyncJobContext)>
    {
        
    }
}
