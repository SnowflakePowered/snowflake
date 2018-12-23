using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Model.Game
{
    public struct PlatformId : IEquatable<PlatformId>, IEquatable<string>
    {
        public string PlatformIdString { get; }
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

    }
}
