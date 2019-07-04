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

namespace Snowflake.Configuration.Serialization
{
    public class ConfigurationSerializationTests
    {
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
            IAbstractConfigurationNode dolphinList = list["#dolphin"];

            var iniSerializer = new SimpleIniConfigurationSerializer();
            var xmlSerializer = new SimpleXmlConfigurationSerializer("Config");
            var cfgSerializer = new SimpleCfgConfigurationSerializer();
            var jsonSerializer = new SimpleJsonConfigurationSerializer();

            string outputIni = iniSerializer.Serialize(dolphinList);
            string outputXml = xmlSerializer.Serialize(dolphinList);
            string outputJson = jsonSerializer.Serialize(dolphinList);
            string outputCfg = cfgSerializer.Serialize(dolphinList);
        }
    }
}
