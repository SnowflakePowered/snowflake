using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Framework.Remoting.GraphQL.RelayMutations
{
    public abstract class RelayMutationBase
    {
        public string ClientMutationID { get; set; }
    }
}
