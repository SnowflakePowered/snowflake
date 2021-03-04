using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Snowflake.Configuration.Serialization.Serializers.Implementations;
using Snowflake.Configuration.Tests;
using Xunit;
using Zio;
using Zio.FileSystems;
using FS = Snowflake.Filesystem;

using Newtonsoft.Json.Linq;
using System.Xml.Linq;
using System.Linq;
using IniParser.Parser;
using System.Collections.Immutable;

namespace Snowflake.Configuration.Serialization
{
    public class FlatteningVisitor : ConfigurationTreeVisitor
    {
        protected override IAbstractConfigurationNode Visit(ListConfigurationNode node)
        {
            var flattened = new List<IAbstractConfigurationNode>();
            var nodesToAdd = new Stack<IAbstractConfigurationNode>();

            foreach (var childNode in node.Value)
            {
                nodesToAdd.Push(childNode);
            }

            while (nodesToAdd.Any())
            {
                var childNode = nodesToAdd.Pop();
                if (childNode is ListConfigurationNode listNode)
                {
                    foreach (var nextChildNode in listNode.Value)
                    {
                        nodesToAdd.Push(nextChildNode);
                    }
                    continue;
                }
                var visitedNode = this.Visit(childNode);
                if (visitedNode is NilConfigurationNode)
                    continue;
                flattened.Add(this.Visit(childNode));
            }

            return node with { Value = ImmutableArray.CreateRange(flattened) };
        }
    }

    public class ConfigurationVisitorTests
    {
        [Fact]
        public void FlattenVisitor_Test()
        {
            var configuration =
              new ConfigurationCollection<ExampleConfigurationCollection>(new ConfigurationValueCollection());

            var fs = new PhysicalFileSystem();
            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            var dir = new FS.Directory("test", pfs, pfs.GetDirectoryEntry("/"));
            dir.OpenDirectory("program")
              .OpenFile("RMGE01.wbfs").OpenStream().Close();
            configuration.Configuration.ExampleConfiguration.FullscreenResolution = FullscreenResolution.Resolution1152X648;
            var context = new ConfigurationTraversalContext(("game", dir));

            var list = context.TraverseCollection(configuration);
            IAbstractConfigurationNode dolphinList = list["#dolphin"];
            var flattenedList = new FlatteningVisitor().Visit(dolphinList);
            Assert.Equal(16, ((ListConfigurationNode)flattenedList).Value.Length);
        }
    }
}
