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
            this.PlatformIdString = id;
        }

        public bool Equals(PlatformId other)
        {
            return other.PlatformIdString.Equals(this.PlatformIdString);
        }

        public bool Equals(string other)
        {
            return other.Equals(this.PlatformIdString);
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

        public static bool operator ==(string x, PlatformId y) => x == y.PlatformIdString;
        public static bool operator !=(string x, PlatformId y) => x != y.PlatformIdString;
        public static bool operator ==(PlatformId x, string y) => x.PlatformIdString == y;
        public static bool operator !=(PlatformId x, string y) => x.PlatformIdString == y;

        public static implicit operator PlatformId(string other) => new PlatformId(other);
        public static implicit operator string(PlatformId id) => id.PlatformIdString;
    }
}
