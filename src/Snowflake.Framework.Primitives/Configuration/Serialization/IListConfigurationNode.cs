using System.Collections.Generic;
namespace Snowflake.Configuration.Serialization
{
    public interface IListConfigurationNode : IAbstractConfigurationNode<IReadOnlyList<IAbstractConfigurationNode>>
    {

    }
}
