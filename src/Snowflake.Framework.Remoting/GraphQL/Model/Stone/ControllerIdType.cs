﻿using HotChocolate.Language;
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
    : ScalarType
    {
        public override Type ClrType => typeof(ControllerId);

        /// <summary>
        /// GraphQL Scalar Definition for a <see cref="ControllerId"/>
        /// </summary>
        public ControllerIdType()
            : base("ControllerId")
        {
            Description = "A Stone ControllerId must be of the form /^[A-Z0-9_]+(_CONTROLLER|_DEVICE|_LAYOUT)/ and represents a specific Stone controller layout.";
        }

        // define which literals this type can be parsed from.
        public override bool IsInstanceOfType(IValueNode literal)
        {
            if (literal == null)
            {
                throw new ArgumentNullException(nameof(literal));
            }

            return literal is StringValueNode
                || literal is NullValueNode;
        }

        // define how a literal is parsed to the native .NET type.
        public override object ParseLiteral(IValueNode literal)
        {
            if (literal == null)
            {
                throw new ArgumentNullException(nameof(literal));
            }

            if (literal is StringValueNode stringLiteral)
            {
                return (ControllerId)stringLiteral.Value;
            }

            if (literal is NullValueNode)
            {
                return null;
            }

            throw new ArgumentException(
                "ControllerId type can only be parsed from string literals.",
                nameof(literal));
        }

        // define how a native type is parsed into a literal,
        public override IValueNode ParseValue(object value)
        {
            if (value == null)
            {
                return new NullValueNode(null);
            }

            if (value is ControllerId p)
            {
                return new StringValueNode(null, p, false);
            }

            throw new ArgumentException(
                "The specified value has to be a ControllerId in order to be parsed");
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
