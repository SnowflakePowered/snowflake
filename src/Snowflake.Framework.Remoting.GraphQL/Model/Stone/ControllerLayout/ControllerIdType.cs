using HotChocolate.Language;
using HotChocolate.Types;
using Snowflake.Input.Controller;
using Snowflake.Model.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Remoting.GraphQL.Model.Stone.ControllerLayout
{
    /// <summary>
    /// GraphQL Scalar Definition
    /// </summary>
    public sealed class ControllerIdType
    : ScalarType<ControllerId, StringValueNode>
    {
        /// <summary>
        /// GraphQL Scalar Definition for a <see cref="ControllerId"/>
        /// </summary>
        public ControllerIdType()
            : base("ControllerId", BindingBehavior.Implicit)
        {
            Description = "A Stone ControllerId must be of the form /^[A-Z0-9_]+(_CONTROLLER|_DEVICE|_LAYOUT)/ and represents a specific Stone controller layout.";
        }

        protected override ControllerId ParseLiteral(StringValueNode literal)
        {
            return (ControllerId)literal.Value;
        }

        protected override StringValueNode ParseValue(ControllerId value)
        {
            return new StringValueNode(null, value, false);
        }

        // define the result serialization. A valid output must be of the following .NET types:
        // System.String, System.Char, System.Int16, System.Int32, System.Int64,
        // System.Float, System.Double, System.Decimal and System.Boolean
        public bool TrySerialize(object value, out object serialized)
        {
            if (value is ControllerId p)
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
                value = (ControllerId)s;
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
