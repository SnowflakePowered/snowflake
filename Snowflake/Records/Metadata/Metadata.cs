using System;
using Snowflake.Utility;

namespace Snowflake.Records.Metadata
{
    public class Metadata : IMetadata
    {
        public string Key { get; }
        public string Value { get; }
        public Guid MetadataGuid { get; }
        public Guid Element { get; }

        public Metadata(string key, string value, Guid element)
        {
            this.Key = key;
            this.Value = value;
            this.Element = element;
            this.MetadataGuid = GuidUtility.Create(this.Element, this.Key);
        }

        public Metadata(string key, string value, IMetadataAssignable element) : this(key, value, element.Guid)
        {
        }

        public bool Equals(IMetadata metadata) => this.MetadataGuid == metadata.MetadataGuid;
        public override int GetHashCode() => this.MetadataGuid.GetHashCode();

        public override bool Equals(object metadata)
        {
            IMetadata m = metadata as IMetadata;

            if (m == null)
            {
                return false;
            }

            // Return true if the fields match:
            return m.MetadataGuid == this.MetadataGuid;
        }

        public static bool operator ==(Metadata metadataX, IMetadata metadataY) => metadataX.Equals(metadataY);
        public static bool operator !=(Metadata metadataX, IMetadata metadataY) => !(metadataX == metadataY);
    }
}
