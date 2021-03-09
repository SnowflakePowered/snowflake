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

namespace Snowflake.Configuration.Serialization.Tests
{
    public class ConfigurationSerializationTests
    {
        [Fact]
        public void CollectionToAbstractConfigurationNodeIniSerializer_Test()
        {
            var configuration =
              new ConfigurationCollection<ExampleConfigurationCollection>(new ConfigurationValueCollection());

            var fs = new PhysicalFileSystem();
            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            string test = Path.GetRandomFileName();
            var dir = new FS.Directory(test, pfs, pfs.GetDirectoryEntry("/"));
            dir.OpenDirectory("program")
              .OpenFile("RMGE01.wbfs").OpenStream().Close();
            configuration.Configuration.ExampleConfiguration.FullscreenResolution = FullscreenResolution.Resolution1152X648;
            var context = new ConfigurationTraversalContext(("game", dir));

            var list = context.TraverseCollection(configuration);
            IAbstractConfigurationNode dolphinList = list["#dolphin"];

            var iniSerializer = new SimpleIniConfigurationSerializer();
            string outputIni = iniSerializer.Visit(dolphinList);
            var parser = new IniDataParser();
            var data = parser.Parse(outputIni);
            Assert.NotEmpty(data.Sections);
        }

        [Fact]
        public void CollectionToAbstractConfigurationNodeCfgSerializer_Test()
        {
            var configuration =
              new ConfigurationCollection<ExampleConfigurationCollection>(new ConfigurationValueCollection());

            var fs = new PhysicalFileSystem();
            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            string test = Path.GetRandomFileName();
            var dir = new FS.Directory(test, pfs, pfs.GetDirectoryEntry("/"));
            dir.OpenDirectory("program")
              .OpenFile("RMGE01.wbfs").OpenStream().Close();
            configuration.Configuration.ExampleConfiguration.FullscreenResolution = FullscreenResolution.Resolution1152X648;
            var context = new ConfigurationTraversalContext(("game", dir));

            var list = context.TraverseCollection(configuration);
            IAbstractConfigurationNode dolphinList = list["#dolphin"];

            var cfgSerializer = new SimpleCfgConfigurationSerializer();
            string outputCfg = cfgSerializer.Visit(dolphinList);
            Assert.NotEqual(string.Empty, outputCfg);
            // todo: test cfg parse
        }

        [Fact]
        public void CollectionToAbstractConfigurationNodeXmlSerializer_Test()
        {
            var configuration =
              new ConfigurationCollection<ExampleConfigurationCollection>(new ConfigurationValueCollection());

            var fs = new PhysicalFileSystem();
            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            string test = Path.GetRandomFileName();
            var dir = new FS.Directory(test, pfs, pfs.GetDirectoryEntry("/"));
            dir.OpenDirectory("program")
              .OpenFile("RMGE01.wbfs").OpenStream().Close();
            configuration.Configuration.ExampleConfiguration.FullscreenResolution = FullscreenResolution.Resolution1152X648;
            var context = new ConfigurationTraversalContext(("game", dir));
            var list = context.TraverseCollection(configuration);
            IAbstractConfigurationNode dolphinList = list["#dolphin"];

            var xmlSerializer = new SimpleXmlConfigurationSerializer("Config");
           
            string outputXml = xmlSerializer.Visit(dolphinList);
            XDocument doc = XDocument.Parse(outputXml);
            Assert.NotEmpty(doc.Nodes());
        }

        [Fact]
        public void CollectionToAbstractConfigurationNodeJsonSerializer_Test()
        {
            var configuration =
              new ConfigurationCollection<ExampleConfigurationCollection>(new ConfigurationValueCollection());

            var fs = new PhysicalFileSystem();
            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            string test = Path.GetRandomFileName();
            var dir = new FS.Directory(test, pfs, pfs.GetDirectoryEntry("/")); dir.OpenDirectory("program")
              .OpenFile("RMGE01.wbfs").OpenStream().Close();
            configuration.Configuration.ExampleConfiguration.FullscreenResolution = FullscreenResolution.Resolution1152X648;
            var context = new ConfigurationTraversalContext(("game", dir));

            var list = context.TraverseCollection(configuration);
            IAbstractConfigurationNode dolphinList = list["#dolphin"];

            var jsonSerializer = new SimpleJsonConfigurationSerializer();
            string outputJson = jsonSerializer.Visit(dolphinList);
            var jtoken = JToken.Parse(outputJson);
            Assert.True(jtoken.HasValues);
        }


        [Fact]
        public void CollectionToAbstractConfigurationNodeCemuXmlSerializer_Test()
        {
            var configuration =
              new ConfigurationCollection<CemuConfigurationCollection>(new ConfigurationValueCollection());

            var fs = new PhysicalFileSystem();
            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            string test = Path.GetRandomFileName();
            var dir = new FS.Directory(test, pfs, pfs.GetDirectoryEntry("/"));
            var context = new ConfigurationTraversalContext(("game", dir));
            var list = context.TraverseCollection(configuration);
            IAbstractConfigurationNode dolphinList = list["#cemu"];

            var xmlSerializer = new SimpleXmlConfigurationSerializer("content");

            string outputXml = xmlSerializer.Visit(dolphinList);
            XDocument doc = XDocument.Parse(outputXml);
            Assert.NotEmpty(doc.Nodes());
        }
    }
}
