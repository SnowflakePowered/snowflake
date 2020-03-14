using HotChocolate.Language;
using HotChocolate.Types;
using Snowflake.Model.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Framework.Remoting.GraphQL.Model.Stone
{
    /// <summary>
    /// GraphQL Scalar Definition
    /// </summary>
    public sealed class PlatformIdType
    : ScalarType<PlatformId, StringValueNode>
    {
        /// <summary>
        /// GraphQL Scalar Definition for a <see cref="PlatformId"/>
        /// </summary>
        public PlatformIdType()
            : base("PlatformId")
        {
            Description = "A Stone PlatformId must be of the form MANUFACTURER_SHORTNAME and represents a specific Stone platform.";
        }

        protected override PlatformId ParseLiteral(StringValueNode literal)
        {
            if (literal == null)
            {
                throw new ArgumentNullException(nameof(literal));
            }
            return (PlatformId)literal.Value;
        }

        protected override StringValueNode ParseValue(PlatformId value)
        {
            return new StringValueNode(null, value, false);
        }
    }
}
