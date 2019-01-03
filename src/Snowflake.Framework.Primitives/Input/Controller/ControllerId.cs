using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Input.Controller
{
    public struct ControllerId : IEquatable<ControllerId>, IEquatable<string>
    {
        private string ControllerIdString { get; }

        public ControllerId(string id)
        {
            this.ControllerIdString = id;
        }

        public bool Equals(ControllerId other)
        {
            if (other.ControllerIdString == null) return false;
            return other.ControllerIdString.Equals(this.ControllerIdString);
        }

        public bool Equals(string other)
        {
            return other.Equals(this.ControllerIdString);
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

        public static bool operator ==(ControllerId x, ControllerId y) => x.ControllerIdString == y.ControllerIdString;
        public static bool operator !=(ControllerId x, ControllerId y) => x.ControllerIdString != y.ControllerIdString;

        public static bool operator ==(string x, ControllerId y) => x == y.ControllerIdString;
        public static bool operator !=(string x, ControllerId y) => x != y.ControllerIdString;
        public static bool operator ==(ControllerId x, string y) => x.ControllerIdString == y;
        public static bool operator !=(ControllerId x, string y) => x.ControllerIdString == y;

        public static implicit operator ControllerId(string other) => new ControllerId(other);
        public static implicit operator string(ControllerId id) => id.ControllerIdString;
    }
}
