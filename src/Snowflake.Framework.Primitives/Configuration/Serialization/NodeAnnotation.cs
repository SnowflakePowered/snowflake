using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Configuration.Serialization
{
    /// <summary>
    /// Represents an annotation on an <see cref="IAbstractConfigurationNode"/>
    /// </summary>
    public struct NodeAnnotation
    {
        /// <summary>
        /// Create a new annotation with the specified <paramref name="kind"/> and <paramref name="value"/>.
        /// </summary>
        /// <param name="kind">The kind of the annotation.</param>
        /// <param name="value">The value of the annotation.</param>
        public NodeAnnotation(string kind, object value)
        {
            this.Kind = kind;
            this.Value = value;
        }

        /// <summary>
        /// Create a new annotation with the specified <paramref name="kind"/>.
        /// </summary>
        /// <param name="kind">The kind of the annotation.</param>
        public NodeAnnotation(string kind)
        {
            this.Kind = kind;
            this.Value = null;
        }

        /// <summary>
        /// The kind of the annotation.
        /// </summary>
        public string Kind { get; }

        /// <summary>
        /// The value of the annotation.
        /// </summary>
        public object? Value { get; }

        /// <inheritdoc />
        public override int GetHashCode() => HashCode.Combine(this.Kind, this.Value);
    }
}
