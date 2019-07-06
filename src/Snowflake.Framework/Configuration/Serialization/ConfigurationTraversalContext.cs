 using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Reflection;
using System.Linq;
using Snowflake.Configuration.Attributes;
using Snowflake.Filesystem;
using Snowflake.Configuration.Extensions;
using Castle.Core.Internal;
using Snowflake.Configuration.Input;

namespace Snowflake.Configuration.Serialization
{
    public sealed class ConfigurationTraversalContext
    {
        /// <summary>
        /// Special string for the null target that is never serialized.
        /// </summary>
        public static string NullTarget = "#null";

        public ConfigurationTraversalContext(IDirectory pathResolutionContext)
        {
            this.PathResolutionContext = pathResolutionContext;
        }

        public IDirectory PathResolutionContext { get; }

        private static IEnumerable<IConfigurationTarget> ResolveConfigurationTargets(IConfigurationCollection collection)
        {
            var targets = collection.GetType().GetPublicAttributes<ConfigurationTargetAttribute>();
            var rootTargets = targets.Where(t => t.IsRoot);
            List<ConfigurationTarget> configurationTargets = new List<ConfigurationTarget>();

            foreach (var rootTargetAttr in rootTargets)
            {
                var rootTarget = new ConfigurationTarget(rootTargetAttr.TargetName, rootTargetAttr.TargetTransformer);

                Queue<ConfigurationTarget> targetsToProcess = new Queue<ConfigurationTarget>();
                targetsToProcess.Enqueue(rootTarget);

                while (targetsToProcess.Count > 0)
                {
                    var target = targetsToProcess.Dequeue();
                    foreach (var childTargets in targets.Where(t => t.ParentTarget == target.TargetName)
                        .Select(t => new ConfigurationTarget(t.TargetName, t.TargetTransformer)))
                    {
                        if (target.ChildTargets.ContainsKey(childTargets.TargetName))
                            throw new ArgumentException("Target name already exists in the graph!");
                        target.ChildTargets.Add(childTargets.TargetName, childTargets);
                        targetsToProcess.Enqueue(childTargets);
                    }
                }

                configurationTargets.Add(rootTarget);
            }

            return configurationTargets;
        }

        private static ListConfigurationNode BuildConfigNode(IConfigurationTarget target,
            IDictionary<string, (ConfigurationTargetAttribute _, List<IAbstractConfigurationNode> nodes)> flatTargets)
        {
            List<IAbstractConfigurationNode> targetNodes = new List<IAbstractConfigurationNode>();
            targetNodes.AddRange(flatTargets[target.TargetName].nodes);
            foreach (var child in target.ChildTargets)
            {
                targetNodes.Add(BuildConfigNode(child.Value, flatTargets));
            }
            return new ListConfigurationNode(target.TargetName, targetNodes.AsReadOnly());
        }

        public IReadOnlyDictionary<string, IAbstractConfigurationNode<IReadOnlyList<IAbstractConfigurationNode>>>
            TraverseCollection(IConfigurationCollection collection)
        {
            if (collection.GetType().IsGenericType
                && collection.GetType().GetGenericTypeDefinition() == typeof(ConfigurationCollection<>))
                throw new InvalidOperationException("Can not traverse on the wrapping type of ConfigurationCollection<T>, you must traverse on the Configuration property.");

            // Get each target for each section.
            var targetMappings = collection
                .GetType()
                .GetInterfaces()
                // This is a hack to get the declaring interface.
                .FirstOrDefault(i => i.GetCustomAttributes<ConfigurationTargetAttribute>().Count() != 0)?
                .GetPublicProperties()
                .Where(props => props.GetIndexParameters().Length == 0
                        && props.PropertyType.GetInterfaces().Contains(typeof(IConfigurationSection)))
                .ToDictionary(p => p.Name, p => p.GetAttribute<ConfigurationTargetMemberAttribute>()?.TargetName ?? NullTarget);

            // If there are no targets, then there is nothing to do.
            if (targetMappings == null) return new Dictionary<string, IAbstractConfigurationNode<IReadOnlyList<IAbstractConfigurationNode>>>();

            var flatTargets = collection.GetType().GetPublicAttributes<ConfigurationTargetAttribute>()
                .ToDictionary(t => t.TargetName, t => (target: t, nodes: new List<IAbstractConfigurationNode>()));

            foreach (var (key, value) in collection)
            {
                string targetName = targetMappings[key];
                if (!flatTargets.ContainsKey(targetName) || targetName == ConfigurationTraversalContext.NullTarget) continue;
                flatTargets[targetName].nodes.Add(new ListConfigurationNode(value.Descriptor.SectionName, this.TraverseSection(value).AsReadOnly()));
            }

            var targets = ConfigurationTraversalContext.ResolveConfigurationTargets(collection);

            Dictionary<string, IAbstractConfigurationNode<IReadOnlyList<IAbstractConfigurationNode>>> rootNodes = new Dictionary<string, IAbstractConfigurationNode<IReadOnlyList<IAbstractConfigurationNode>>>();

            foreach (var rootTarget in targets)
            {
                var node = ConfigurationTraversalContext.BuildConfigNode(rootTarget, flatTargets);
                rootNodes.Add(node.Key, node);
            }

            return rootNodes;
        }

        public IAbstractConfigurationNode<IReadOnlyList<IAbstractConfigurationNode>>
            TraverseInputTemplate(IInputTemplate template,
            IInputMapping mapping,
            int index,
            string indexer = "{N}")
        {
            var configNodes = this.TraverseSection(template, true, indexer, index);
            foreach (var inputOption in template.Options)
            {
                configNodes.Add(new ControllerElementConfigurationNode(
                    inputOption.OptionName.Replace(indexer, Convert.ToString(index)),
                    inputOption.TargetElement, 
                    mapping[inputOption.TargetElement],
                    inputOption.DeviceType));
            }
            return new ListConfigurationNode(template.Descriptor.SectionName.Replace(indexer, 
                Convert.ToString(index)),
                configNodes.AsReadOnly());
        }

        private List<IAbstractConfigurationNode> TraverseSection(IConfigurationSection section,
            bool useIndexer　= false,
            string indexer = "{N}", 
            int index = 0)
        {
            var nodes = new List<IAbstractConfigurationNode>();
            foreach (var (key, value) in section)
            {
                string serializedKey = key.OptionName;
                if (useIndexer) serializedKey = serializedKey.Replace(indexer, Convert.ToString(index));
                if (key.Flag) continue;

                if (key.IsPath && value.Value is string path)
                {
                    string directoryName = Path.GetDirectoryName(path);
                    var directory = (Filesystem.Directory)this.PathResolutionContext.OpenDirectory(directoryName);
                    var file = directory.OpenFile(path);

                    IAbstractConfigurationNode pathNode = key.PathType switch
                    {
                        PathType.Directory => new StringConfigurationNode(serializedKey, directory.GetPath().FullName),
#pragma warning disable CS0618 // Type or member is obsolete
                        PathType.File => new StringConfigurationNode(serializedKey, file.GetFilePath().FullName),
                        PathType.Either => file.Created ? new StringConfigurationNode(serializedKey, file.GetFilePath().FullName) :
                            new StringConfigurationNode(serializedKey, directory.GetPath().FullName),
#pragma warning restore CS0618 // Type or member is obsolete
                        PathType.Raw => new StringConfigurationNode(serializedKey, path),
                        // UnknownConfigurationNodes are ignored in most serializers.
                        _ => new UnknownConfigurationNode(serializedKey, path) 
                            as IAbstractConfigurationNode // hack to allow type inference,
                    };
                    nodes.Add(pathNode);
                    continue;
                }

                IAbstractConfigurationNode node = value.Value switch
                {
                    bool rawVal => new BooleanConfigurationNode(serializedKey, rawVal),

                    float rawVal => new DecimalConfigurationNode(serializedKey, rawVal),
                    double rawVal => new DecimalConfigurationNode(serializedKey, rawVal),

                    sbyte rawVal => new IntegralConfigurationNode(serializedKey, rawVal),
                    short rawVal => new IntegralConfigurationNode(serializedKey, rawVal),
                    int rawVal => new IntegralConfigurationNode(serializedKey, rawVal),
                    long rawVal => new IntegralConfigurationNode(serializedKey, rawVal),

                    byte rawVal => new IntegralConfigurationNode(serializedKey, rawVal),
                    ushort rawVal => new IntegralConfigurationNode(serializedKey, rawVal),
                    uint rawVal => new IntegralConfigurationNode(serializedKey, rawVal),

                    string rawVal => new StringConfigurationNode(serializedKey, rawVal),
                    char rawVal => new StringConfigurationNode(serializedKey, rawVal.ToString()),

                    Enum rawVal => new EnumConfigurationNode(serializedKey, rawVal, rawVal.GetType()),

                    _ => new UnknownConfigurationNode(serializedKey, value.Value)
                        as IAbstractConfigurationNode, // hack to allow type inference
                };
                nodes.Add(node);
            }

            return nodes;
        }
    }
}
