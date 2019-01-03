using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Model.Game
{
    public struct PlatformId : IEquatable<PlatformId>, IEquatable<string>
    {
        private string PlatformIdString { get; }

        public PlatformId(string id)
        {
            this.PlatformIdString = id?.ToUpperInvariant() ?? "NULL_PLATFORM";
        }

        public bool Equals(PlatformId other)
        {
            if (other.PlatformIdString == null) return false;
            return other.PlatformIdString.Equals(this.PlatformIdString, StringComparison.InvariantCultureIgnoreCase);
        }

        public bool Equals(string other)
        {
            if (other == null) return false;
            return other.Equals(this.PlatformIdString, StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool Equals(object other)
        {
            if (other is PlatformId p)
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
            return this.PlatformIdString;
        }

        public static bool operator ==(PlatformId x, PlatformId y) => x.PlatformIdString == y.PlatformIdString;
        public static bool operator !=(PlatformId x, PlatformId y) => x.PlatformIdString != y.PlatformIdString;

        public static bool operator ==(string x, PlatformId y) => x.ToUpperInvariant() == y.PlatformIdString;
        public static bool operator !=(string x, PlatformId y) => x.ToUpperInvariant() != y.PlatformIdString;
        public static bool operator ==(PlatformId x, string y) => x.PlatformIdString == y.ToUpperInvariant();
        public static bool operator !=(PlatformId x, string y) => x.PlatformIdString != y.ToUpperInvariant();

        public static implicit operator PlatformId(string other) => new PlatformId(other);
        public static implicit operator string(PlatformId id) => id.PlatformIdString;
    }
}
