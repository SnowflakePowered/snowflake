using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Configuration.Serialization
{
    /// <summary>
    /// Implements stateless immutable <see cref="IConfigurationVisitor{TOutput}"/> with visitor semantics
    /// to reshape a <see cref="IAbstractConfigurationNode"/> returned by <see cref="IConfigurationTraversalContext"/>
    /// or another <see cref = "ConfigurationTreeVisitor"/>.
    /// 
    /// <para>
    /// By convention, returning a <see cref="NilConfigurationNode"/> should remove the node from the tree. <strong>This should especially be checked if implementing
    /// <see cref="Visit(ListConfigurationNode)"/>.</strong>
    /// </para>
    /// </summary>
    public abstract class ConfigurationTreeVisitor : IConfigurationVisitor<IAbstractConfigurationNode>
    {
        public IAbstractConfigurationNode Visit(IAbstractConfigurationNode node)
        {
            return node switch
            {
                BooleanConfigurationNode childNode => this.Visit(childNode),
                DecimalConfigurationNode childNode => this.Visit(childNode),
                DeviceCapabilityElementConfigurationNode childNode => this.Visit(childNode),
                EnumConfigurationNode childNode => this.Visit(childNode),
                IntegralConfigurationNode childNode => this.Visit(childNode),
                StringConfigurationNode childNode => this.Visit(childNode),
                ListConfigurationNode childNode => this.Visit(childNode),
                UnknownConfigurationNode childNode => this.Visit(childNode),
                _ => node
            };
        }

        protected virtual IAbstractConfigurationNode Visit(BooleanConfigurationNode node) => node;
        protected virtual IAbstractConfigurationNode Visit(DecimalConfigurationNode node) => node;
        protected virtual IAbstractConfigurationNode Visit(DeviceCapabilityElementConfigurationNode node) => node;
        protected virtual IAbstractConfigurationNode Visit(EnumConfigurationNode node) => node;
        protected virtual IAbstractConfigurationNode Visit(IntegralConfigurationNode node) => node;
        protected virtual IAbstractConfigurationNode Visit(ListConfigurationNode node) => node;
        protected virtual IAbstractConfigurationNode Visit(StringConfigurationNode node) => node;
        protected virtual IAbstractConfigurationNode Visit(UnknownConfigurationNode node) => node;
    }
}
