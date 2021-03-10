using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Text;
using Snowflake.Configuration.Input;
using Snowflake.Configuration.Tests;
using Snowflake.Input.Controller;
using Snowflake.Input.Device;
using Snowflake.Input.Tests;
using Snowflake.Services;
using Xunit;
using Zio;
using Zio.FileSystems;
using FS = Snowflake.Filesystem;

namespace Snowflake.Configuration.Serialization
{
    public class ConfigurationTraversalContextTests
    {

        [Fact]
        public void PathNamespaceNotExists_Test()
        {
            var configuration =
             new ConfigurationCollection<ExampleConfigurationCollection>(new ConfigurationValueCollection());

            var fs = new PhysicalFileSystem();
            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            var dir = new FS.Directory("test", pfs, pfs.GetDirectoryEntry("/"));

            var context = new ConfigurationTraversalContext();
            Assert.Throws<KeyNotFoundException>(() => context.TraverseCollection(configuration));
        }

        [Fact]
        public void PathNamespaceNotSpecified_Test()
        {
            var configuration =
             new ConfigurationCollection<MissingPathConfigurationCollection>(new ConfigurationValueCollection());

            var fs = new PhysicalFileSystem();
            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            var dir = new FS.Directory("test", pfs, pfs.GetDirectoryEntry("/"));

            var context = new ConfigurationTraversalContext();
            Assert.Throws<ArgumentException>(() => context.TraverseCollection(configuration));
        }

        [Fact]
        public void InputTemplateToAbstractConfigurationNode_Test()
        {
            var mapcol = new ControllerElementMappingProfile("Keyboard",
                            "TEST_CONTROLLER",
                            InputDriver.Keyboard,
                            IDeviceEnumerator.VirtualVendorID,
                            new XInputDeviceInstance(0).DefaultLayout);
            IDeviceInputMapping mapping = new TestInputMapping();

            var input =
             new InputConfiguration<IRetroArchInput>(mapcol);

            var fs = new PhysicalFileSystem();
            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            var dir = new FS.Directory("test", pfs, pfs.GetDirectoryEntry("/"));
            dir.OpenDirectory("program")
              .OpenFile("RMGE01.wbfs").OpenStream(FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite).Close();
            var context = new ConfigurationTraversalContext(("game", dir));
            var node = context.TraverseInputTemplate(input, mapping, 0);
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
            dir.OpenDirectory("program")
              .OpenFile("RMGE01.wbfs").OpenStream(FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite).Close();
            configuration.Configuration.ExampleConfiguration.FullscreenResolution = FullscreenResolution.Resolution1152X648;
            var context = new ConfigurationTraversalContext(("game", dir));

            var list = context.TraverseCollection(configuration);
            Assert.Equal(2, list.Count);
            Assert.Equal(2, list["#dolphin"].Value.Length);
            Assert.DoesNotContain("TestCycle1", list.Keys);
            Assert.DoesNotContain("TestCycle2", list.Keys);

            var dolphinList = list["#dolphin"];
            foreach (var node in dolphinList.Value)
            {
                if (node.Key == "Display")
                {
                    var confList = (node as ListConfigurationNode).Value;
                    Assert.Equal(8, confList.Length);
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

                    Assert.Equal("ISODir", confList[6].Key);
                    Assert.IsType<StringConfigurationNode>(confList[6]);

                    Assert.Equal("InternalCpuRatio", confList[7].Key);
                    Assert.IsType<DecimalConfigurationNode>(confList[7]);
                }

                if (node.Key == "TestNestedSection")
                {
                    Assert.Equal("TestNestedNestedSection", (node as ListConfigurationNode).Value[1].Key);
                }
            }
        }

        [Fact]
        public void CollectionToAbstractConfigurationNodeWithExtraNodes_Test()
        {
            var configuration =
              new ConfigurationCollection<ExampleConfigurationCollection>(new ConfigurationValueCollection());

            var fs = new PhysicalFileSystem();
            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            var dir = new FS.Directory("test", pfs, pfs.GetDirectoryEntry("/"));
            dir.OpenDirectory("program")
              .OpenFile("RMGE01.wbfs").OpenStream(FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite).Close();
            configuration.Configuration.ExampleConfiguration.FullscreenResolution = FullscreenResolution.Resolution1152X648;
            var context = new ConfigurationTraversalContext(("game", dir));

            var list = context.TraverseCollection(configuration, new (string, IAbstractConfigurationNode)[] 
            {
                ("#dolphin", new IntegralConfigurationNode("TestNestedExtra", 1036)),
                ("#dolphin", new ListConfigurationNode("TestNestedExtraList", ImmutableArray.Create<IAbstractConfigurationNode>
                    (new StringConfigurationNode("TestNestedTwo", "StrVal"))))
            });

            Assert.Equal(2, list.Count);
            Assert.Equal(4, list["#dolphin"].Value.Length);
            Assert.DoesNotContain("TestCycle1", list.Keys);
            Assert.DoesNotContain("TestCycle2", list.Keys);

            var dolphinList = list["#dolphin"];
            foreach (var node in dolphinList.Value)
            {
                if (node.Key == "TestNestedExtra")
                {
                    Assert.Equal(1036, (node as IntegralConfigurationNode).Value);
                }

                if (node.Key == "TestNestedExtraList")
                {
                    Assert.Equal("StrVal", (node as ListConfigurationNode).Value[0].Value);
                }

                if (node.Key == "Display")
                {
                    var confList = (node as ListConfigurationNode).Value;
                    Assert.Equal(8, confList.Length);
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
                    
                    Assert.Equal("ISODir", confList[6].Key);
                    Assert.IsType<StringConfigurationNode>(confList[6]);

                    Assert.Equal("InternalCpuRatio", confList[7].Key);
                    Assert.IsType<DecimalConfigurationNode>(confList[7]);
                }

                if (node.Key == "TestNestedSection")
                {
                    Assert.Equal("TestNestedNestedSection", (node as ListConfigurationNode).Value[1].Key);
                }
            }
        }
    }
}
