using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Scraping
{
    /// <summary>
    /// A <see cref="SeedContent"/> represents
    /// a unit of scraped data with a semantic type and a value.
    /// Two <see cref="SeedContent"/> with the same type and value are considered equal.
    /// </summary>
    public struct SeedContent : IEquatable<SeedContent>
    {
        /// <summary>
        /// By convention, any non root seeds must be a child of the root seed, which
        /// has a type __root.
        /// </summary>
        public static readonly string RootSeedType = "__root";

        /// <summary>
        /// Represents a null value seedcontent.
        /// </summary>
        public static readonly string NullSeedType = "__null";

        /// <summary>
        /// A <see cref="SeedContent"/> is polymorphically equivalent to a
        /// <see cref="ValueTuple{String, String}"/>
        /// </summary>
        /// <param name="seedContent">The <see cref="SeedContent"/> to coerce.</param>
        public static implicit operator (string type, string value)(SeedContent seedContent)
        {
            return (seedContent.Type, seedContent.Value);
        }

        /// <summary>
        /// A <see cref="SeedContent"/> is polymorphically equivalent to a
        /// <see cref="ValueTuple{String, String}"/>
        /// </summary>
        /// <param name="contentTuple">The <see cref="ValueTuple{String, String}"/> to coerce.</param>
        public static implicit operator SeedContent((string type, string value) contentTuple)
        {
            return new SeedContent(contentTuple.type, contentTuple.value);
        }

        /// <summary>
        /// Determines if two <see cref="SeedContent"/> are equal.
        /// </summary>
        /// <param name="x">The  <see cref="SeedContent"/> on the left side of the operator</param>
        /// <param name="y">The  <see cref="SeedContent"/> on the right side of the operator</param>
        /// <returns>If the two<see cref="SeedContent"/> are equal.</returns>
        public static bool operator ==(SeedContent x, SeedContent y)
        {
            return x.Equals(y);
        }

        /// <summary>
        /// Determines if two <see cref="SeedContent"/> are not equal.
        /// </summary>
        /// <param name="x">The  <see cref="SeedContent"/> on the left side of the operator</param>
        /// <param name="y">The  <see cref="SeedContent"/> on the right side of the operator</param>
        /// <returns>If the two<see cref="SeedContent"/> are not equal.</returns>
        public static bool operator !=(SeedContent x, SeedContent y)
        {
            return !(x == y);
        }

        /// <summary>
        /// Gets the semantic type of the content.
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// Gets the string value of the content.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SeedContent"/> struct.
        /// </summary>
        /// <param name="type">The semantic type of the content.</param>
        /// <param name="value">The value of the content.</param>
        public SeedContent(string type, string value)
        {
            this.Type = type ?? SeedContent.NullSeedType;
            this.Value = value ?? SeedContent.NullSeedType;
        }

        /// <summary>
        /// Determines if this <see cref="SeedContent"/> is equal to another.
        /// </summary>
        /// <param name="other">The other <see cref="SeedContent"/></param>
        /// <returns>if the two objects are equal.</returns>
        public bool Equals(SeedContent other)
        {
            return this.Type == other.Type && this.Value == other.Value;
        }

        /// <summary>
        /// Determines if this <see cref="SeedContent"/> is equal to another object.
        /// </summary>
        /// <param name="other">The object to compare.</param>
        /// <returns>if the two objects are equal.</returns>
        public override bool Equals(object? other)
        {
            if (!(other is SeedContent))
            {
                return false;
            }

            return (SeedContent) other == this;
        }

        /// <summary>
        /// Gets the hashcode of this SeedContent.
        /// </summary>
        /// <returns>The hashcode of this SeedContent.</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(this.Type, this.Value);
        }
    }
}
