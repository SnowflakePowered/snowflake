using Snowflake.Configuration.Serialization;
using System;
namespace Snowflake.Configuration.Serialization.Serializers
{
    public interface IConfigurationSerializer<TOutput>
    {
        TOutput Serialize(IAbstractConfigurationNode node);
    }
}