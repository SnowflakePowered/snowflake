using Snowflake.Configuration.Serialization;

namespace Snowflake.Configuration.Serialization.Serializers
{
    public interface IConfigurationSerializer<TOutput>
    {
        TOutput SerializeNode(IAbstractConfigurationNode node);

        void SerializeBlockBegin(IListConfigurationNode block, IConfigurationSerializationContext<TOutput> context);
        void SerializeBlockEnd(IListConfigurationNode block, IConfigurationSerializationContext<TOutput> context);
        void SerializeNode(IBooleanConfigurationNode node, IConfigurationSerializationContext<TOutput> context);
        void SerializeNode(IDecimalConfigurationNode node, IConfigurationSerializationContext<TOutput> context);
        void SerializeNode(IEnumConfigurationNode node, IConfigurationSerializationContext<TOutput> context);
        void SerializeNode(IAbstractConfigurationNode node, IConfigurationSerializationContext<TOutput> context);
        void SerializeNode(IIntegralConfigurationNode node, IConfigurationSerializationContext<TOutput> context);
        void SerializeNode(IListConfigurationNode node, IConfigurationSerializationContext<TOutput> context);
        void SerializeNode(INullConfigurationNode node, IConfigurationSerializationContext<TOutput> context);
        void SerializeNode(IStringConfigurationNode node, IConfigurationSerializationContext<TOutput> context);
    }
}