﻿using System;
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
            var dolphinList = list["#dolphin"];

            var serializer = new SimpleIniSerializer();
            foreach (var node in dolphinList)
            {
                string output = serializer.SerializeNode(node);
            }
           
        }
    }
}
