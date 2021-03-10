using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.RegularExpressions;

namespace Snowflake.Input.Controller
{
    /// <summary>
    /// Represents a Stone controller ID.
    /// </summary>
    public struct ControllerId : IEquatable<ControllerId>, IEquatable<string>, IComparable<string>, IComparable
    {
        private string ControllerIdString { get; }

        internal static readonly Regex ControllerIdRegex = new Regex("^[A-Z0-9_]+(_CONTROLLER|_DEVICE|_LAYOUT)$",
            RegexOptions.Compiled);
        private ControllerId(string id)
        {
            this.ControllerIdString = id?.ToUpperInvariant() ?? "CONTROLLER_UNKNOWN";
            if (!ControllerIdRegex.IsMatch(this.ControllerIdString)) throw new InvalidControllerIdException(id!);
        }

        /// <inheritdoc cref="Equals(System.Object)"/>
        public bool Equals(ControllerId other)
        {
            return other.ControllerIdString != null && 
                   other.ControllerIdString.Equals(this.ControllerIdString, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <inheritdoc />
        public bool Equals(string? other)
        {
            return other != null && other.Equals(this.ControllerIdString, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <inheritdoc />
        public override bool Equals(object? other)
        {
            return other switch {
                ControllerId p => this.Equals(p),
                string s => this.Equals(s),
                _ => false
            };
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return this.ControllerIdString;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCode.Combine(ControllerIdString);
        }

        /// <inheritdoc />
        public int CompareTo([AllowNull] string other)
        {
            return ControllerIdString.CompareTo(other);
        }

        /// <inheritdoc />
        public int CompareTo(object? obj)
        {
            if (obj is ControllerId c) return ControllerIdString.CompareTo(c.ControllerIdString);
            return ControllerIdString.CompareTo(obj);
        }

#pragma warning disable 1591
        public static bool operator ==(ControllerId x, ControllerId y) => x.ControllerIdString == y.ControllerIdString;
        public static bool operator !=(ControllerId x, ControllerId y) => x.ControllerIdString != y.ControllerIdString;
#pragma warning restore 1591

        
        /// <summary>
        /// Implicitly converts from a <see cref="string"/> to a <see cref="ControllerId"/>
        /// </summary>
        /// <param name="other">The string to convert.</param>
        /// <returns>The <see cref="ControllerId"/> the string represents.</returns>
        public static implicit operator ControllerId(string other) => new ControllerId(other);
        
        /// <summary>
        /// Implicitly converts from a <see cref="ControllerId"/> to a string.
        /// </summary>
        /// <param name="id">The <see cref="ControllerId"/> to convert as a string.</param>
        /// <returns>The <see cref="ControllerId"/> as a string.</returns>
        public static implicit operator string(ControllerId id) => id.ControllerIdString;
    }
}
