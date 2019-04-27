using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Snowflake.Input.Controller
{
    public struct ControllerId : IEquatable<ControllerId>, IEquatable<string>
    {
        private string ControllerIdString { get; }

        private static Regex ControllerIdRegex = new Regex("^[A-Z0-9_]+(_CONTROLLER|_DEVICE|_LAYOUT)$",
            RegexOptions.Compiled);
        private ControllerId(string id)
        {
            this.ControllerIdString = id?.ToUpperInvariant() ?? "CONTROLLER_UNKNOWN";
            if (!ControllerIdRegex.IsMatch(this.ControllerIdString)) throw new InvalidControllerIdException(id);
        }

        public bool Equals(ControllerId other)
        {
            if (other.ControllerIdString == null) return false;
            return other.ControllerIdString.Equals(this.ControllerIdString, StringComparison.InvariantCultureIgnoreCase);
        }

        public bool Equals(string other)
        {
            if (other == null) return false;
            return other.Equals(this.ControllerIdString, StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool Equals(object other)
        {
            if (other is ControllerId p)
            {
                return this.Equals(p);
            }

            if (other is string s)
            {
                return this.Equals(s);
            }

            return false;
        }

        public override string ToString()
        {
            return this.ControllerIdString;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ControllerIdString);
        }

        public static bool operator ==(ControllerId x, ControllerId y) => x.ControllerIdString == y.ControllerIdString;
        public static bool operator !=(ControllerId x, ControllerId y) => x.ControllerIdString != y.ControllerIdString;

        public static bool operator ==(string x, ControllerId y) => x.ToUpperInvariant() == y.ControllerIdString;
        public static bool operator !=(string x, ControllerId y) => x.ToUpperInvariant() != y.ControllerIdString;
        public static bool operator ==(ControllerId x, string y) => x.ControllerIdString == y.ToUpperInvariant();
        public static bool operator !=(ControllerId x, string y) => x.ControllerIdString != y.ToUpperInvariant();

        public static implicit operator ControllerId(string other) => new ControllerId(other);
        public static implicit operator string(ControllerId id) => id.ControllerIdString;
    }
}
