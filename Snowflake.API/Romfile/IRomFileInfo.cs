using Snowflake.Records.Metadata;

namespace Snowflake.Romfile
{
    public interface IRomFileInfo
    {
        string Mimetype { get; }
        string Serial { get; }
        string InternalName { get; }
    }
}