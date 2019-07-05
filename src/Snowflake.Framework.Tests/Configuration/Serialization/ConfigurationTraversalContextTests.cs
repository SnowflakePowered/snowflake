using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Snowflake.Configuration.Tests;
using Xunit;
using Zio;
using Zio.FileSystems;
using FS = Snowflake.Filesystem;

namespace Snowflake.Configuration.Serialization
{
    public class ConfigurationTraversalContextTests
    {

        [Fact]
        public void SectionToAbstractConfigurationNode_Test()
        {
            var configuration =
              new ConfigurationCollection<ExampleConfigurationCollection>(new ConfigurationValueCollection());

            var fs = new PhysicalFileSystem();
            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            var dir = new FS.Directory("test", pfs, pfs.GetDirectoryEntry("/"));

            configuration.Configuration.ExampleConfiguration.FullscreenResolution = FullscreenResolution.Resolution1152X648;
            var context = new ConfigurationTraversalContext(dir);

            var list = context.TraverseSection(configuration.Configuration.ExampleConfiguration);

            Assert.Equal(7, list.Count);
            Assert.Equal("FullscreenResolution", list[0].Key);
            Assert.IsType<EnumConfigurationNode>(list[0]);
            Assert.Equal("1152x648", ((EnumConfigurationNode)list[0]).Value);
            Assert.Equal(FullscreenResolution.Resolution1152X648, list[0].Value);

            Assert.Equal("Fullscreen", list[1].Key);
            Assert.IsType<BooleanConfigurationNode>(list[1]);

            Assert.Equal("RenderToMain", list[2].Key);
            Assert.IsType<BooleanConfigurationNode>(list[2]);

            Assert.Equal("RenderWindowWidth", list[3].Key);
            Assert.IsType<IntegralConfigurationNode>(list[3]);

            Assert.Equal("RenderWindowHeight", list[4].Key);
            Assert.IsType<IntegralConfigurationNode>(list[4]);

            Assert.Equal("ISOPath0", list[5].Key);
            Assert.IsType<StringConfigurationNode>(list[5]);

            Assert.Equal("InternalCpuRatio", list[6].Key);
            Assert.IsType<DecimalConfigurationNode>(list[6]);
        }

        [Fact]
        public void CollectionToAbstractConfigurationNode_Test()
        {
            var configuration =
              new ConfigurationCollection<ExampleConfigurationCollection>(new ConfigurationValueCollection());

            var fs = new PhysicalFileSystem();
            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            var dir = new FS.Directory("test", pfs, pfs.GetDirectoryEntry("/"));

            configuration.Configuration.ExampleConfiguration.FullscreenResolution = FullscreenResolution.Resolution1152X648;
            var context = new ConfigurationTraversalContext(dir);

            var list = context.TraverseCollection(configuration.Configuration);
            Assert.Equal(2, list.Count);
            Assert.Equal(2, list["#dolphin"].Value.Count);
            Assert.DoesNotContain("TestCycle1", list.Keys);
            Assert.DoesNotContain("TestCycle2", list.Keys);

            var dolphinList = list["#dolphin"];
            foreach (var node in dolphinList.Value)
            {
                if (node.Key == "Display")
                {
                    var confList = (node as ListConfigurationNode).Value;
                    Assert.Equal(7, confList.Count);
                    Assert.Equal("FullscreenResolution", confList[0].Key);
                    Assert.IsType<EnumConfigurationNode>(confList[0]);
                    Assert.Equal("1152x648", ((EnumConfigurationNode)confList[0]).Value);
                    Assert.Equal(FullscreenResolution.Resolution1152X648, confList[0].Value);

                    Assert.Equal("Fullscreen", confList[1].Key);
                    Assert.IsType<BooleanConfigurationNode>(confList[1]);

                    Assert.Equal("RenderToMain", confList[2].Key);
                    Assert.IsType<BooleanConfigurationNode>(confList[2]);

                    Assert.Equal("RenderWindowWidth", confList[3].Key);
                    Assert.IsType<IntegralConfigurationNode>(confList[3]);

                    Assert.Equal("RenderWindowHeight", confList[4].Key);
                    Assert.IsType<IntegralConfigurationNode>(confList[4]);

                    Assert.Equal("ISOPath0", confList[5].Key);
                    Assert.IsType<StringConfigurationNode>(confList[5]);

                    Assert.Equal("InternalCpuRatio", confList[6].Key);
                    Assert.IsType<DecimalConfigurationNode>(confList[6]);
                }

                if (node.Key == "TestNestedSection")
                {
                    Assert.Equal("TestNestedNestedSection", (node as ListConfigurationNode).Value[0].Key);
                }
            }
        }
    }
}
