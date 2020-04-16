using HotChocolate.Language;
using HotChocolate.Types;
using Snowflake.Model.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Framework.Remoting.GraphQL.Model.Stone.PlatformInfo
{
    /// <summary>
    /// GraphQL Scalar Definition
    /// </summary>
    public sealed class PlatformIdType
    : ScalarType<PlatformId, StringValueNode>
    {
        public PlatformIdType()
            : base("PlatformId", BindingBehavior.Implicit)
        {
            Description = "A Stone PlatformId must be of the form MANUFACTURER_SHORTNAME and represents a specific Stone platform.";
        }

        protected override PlatformId ParseLiteral(StringValueNode literal)
        {
            return (PlatformId)literal.Value;
        }

        protected override StringValueNode ParseValue(PlatformId value)
        {
            return new StringValueNode(null, value, false);
        }

        // define the result serialization. A valid output must be of the following .NET types:
        // System.String, System.Char, System.Int16, System.Int32, System.Int64,
        // System.Float, System.Double, System.Decimal and System.Boolean
        public bool TrySerialize(object value, out object serialized)
        {
            if (value is PlatformId p)
            {
                serialized = (string)p;
                return true;
            }
            serialized = null;
            return false;
        }

        public override bool TryDeserialize(object serialized, out object value)
        {
            if (serialized is null)
            {
                value = null;
                return true;
            }

            if (serialized is string s)
            {
                value = (PlatformId)s;
                return true;
            }

            value = null;
            return false;
        }

        public override object Serialize(object value)
        {
            return this.TrySerialize(value, out var s) ? s : default;
        }
    }
}
