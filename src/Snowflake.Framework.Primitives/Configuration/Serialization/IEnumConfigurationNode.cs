using System;
namespace Snowflake.Configuration.Serialization
{
    public interface IEnumConfigurationNode : IAbstractConfigurationNode<Enum>
    {
        new string Value { get; }
    }
}
