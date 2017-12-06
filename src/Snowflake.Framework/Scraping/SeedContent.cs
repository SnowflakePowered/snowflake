using System;
using System.Collections.Generic;
using System.Text;
namespace Snowflake.Scraping
{
    public struct SeedContent : IEquatable<SeedContent>
    {
        public const string RootSeedType = "__root";

        public static implicit operator (string type, string value) (SeedContent seedContent)
        {
            return (seedContent.Type, seedContent.Value);
        }

        public static implicit operator SeedContent((string type, string value) contentTuple)
        {
            return new SeedContent(contentTuple.type, contentTuple.value);
        }

        public static bool operator ==(SeedContent x, SeedContent y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(SeedContent x, SeedContent y)
        {
            return !(x == y);
        }

        public string Type { get; }
        public string Value { get; }
        public SeedContent(string type, string value)
        {
            this.Type = type;
            this.Value = value;
        }

        public bool Equals(SeedContent other)
        {
            return this.Type == other.Type && this.Value == other.Value;
        }

        public override bool Equals(object other)
        {
            if (!(other is SeedContent))
            {
                return false;
            }

            return (SeedContent)other == this;
        }

        public override int GetHashCode()
        {
            // todo: use System.HashCode when available.
            return (this.Type, this.Value).GetHashCode();
        }

    }
}
