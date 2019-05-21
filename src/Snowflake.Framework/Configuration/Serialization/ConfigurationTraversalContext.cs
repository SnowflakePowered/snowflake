using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Snowflake.Filesystem;

namespace Snowflake.Configuration.Serialization
{
    public sealed class ConfigurationTraversalContext
    {
        public ConfigurationTraversalContext(IDirectory pathResolutionContext)
        {
            this.PathResolutionContext = pathResolutionContext;
        }

        public IDirectory PathResolutionContext { get; }

        public IReadOnlyList<IAbstractConfigurationNode> TraverseSection(IConfigurationSection section)
        {
            var nodes = new List<IAbstractConfigurationNode>();
            foreach (var (key, value) in section)
            {
                string serializedKey = key.OptionName;
                if (key.Flag) continue;

                if (key.IsPath && value.Value is string path)
                {
                    string directoryName = Path.GetDirectoryName(path);
                    var directory = (Filesystem.Directory)this.PathResolutionContext.OpenDirectory(directoryName);
                    var file = directory.OpenFile(path);

                    StringConfigurationNode pathNode = key.PathType switch
                    {
                        PathType.Directory => new StringConfigurationNode(serializedKey, directory.GetPath().FullName),
#pragma warning disable CS0618 // Type or member is obsolete
                        PathType.File => new StringConfigurationNode(serializedKey, file.GetFilePath().FullName),
                        PathType.Either => file.Created ? new StringConfigurationNode(serializedKey, file.GetFilePath().FullName) :
                            new StringConfigurationNode(serializedKey, directory.GetPath().FullName),
                        _ => new StringConfigurationNode(serializedKey, directory.GetPath().FullName) // should never happen.
#pragma warning restore CS0618 // Type or member is obsolete
                    };
                    nodes.Add(pathNode);
                    continue;
                }

                IAbstractConfigurationNode node = value.Value switch
                {
                    null => new NullConfigurationNode(serializedKey),
                    bool rawVal => new BoolConfigurationNode(serializedKey, rawVal),

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

                    _ => new UnknownConfigurationNode(serializedKey, value) 
                        as IAbstractConfigurationNode, // hack to allow type inference
                };
                nodes.Add(node);
            }

            return nodes.AsReadOnly();
        }
    }
}
