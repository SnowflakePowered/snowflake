namespace Snowflake.Identifier
{
    public interface IIdentifiedMetadata
    {
        string IdentifierName { get; }
        string ValueType { get; }
        string Value { get; }
    }
}
