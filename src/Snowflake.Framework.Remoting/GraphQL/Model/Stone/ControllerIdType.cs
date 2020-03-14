using GraphQL.Types;
using HotChocolate.Language;
using HotChocolate.Types;
using Snowflake.Input.Controller;
using Snowflake.Model.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Framework.Remoting.GraphQL.Model.Stone
{
    /// <summary>
    /// GraphQL Scalar Definition
    /// </summary>
    internal sealed class ControllerIdType
    : ScalarType<ControllerId, StringValueNode>
    {
        /// <summary>
        /// GraphQL Scalar Definition for a <see cref="ControllerId"/>
        /// </summary>
        public ControllerIdType()
            : base("ControllerId")
        {
            Description = "A Stone ControllerId must be of the form /^[A-Z0-9_]+(_CONTROLLER|_DEVICE|_LAYOUT)/ and represents a specific Stone controller layout.";
        }
        protected override ControllerId ParseLiteral(StringValueNode literal)
        {
            if (literal == null)
            {
                throw new ArgumentNullException(nameof(literal));
            }
            return literal.Value;
        }

        protected override StringValueNode ParseValue(ControllerId value)
        {
            return new StringValueNode(null, value, false);
        }
    }
}
