using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Remoting.GraphQL.FrameworkQueries.Mutations.Relay
{
    /// <summary>
    /// Base class for relay mutation input type and payload.
    /// </summary>
    public abstract class RelayMutationBase
    {
        /// <summary>
        /// The Relay Classic Client Mutation ID.
        /// </summary>
        public string ClientMutationID { get; set; }
    }
}
