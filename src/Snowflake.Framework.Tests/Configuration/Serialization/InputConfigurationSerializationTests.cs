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

using Newtonsoft.Json.Linq;
using System.Xml.Linq;
using System.Linq;
using IniParser.Parser;
using Newtonsoft.Json;
using Snowflake.Tests;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Configuration.Input;
using Snowflake.Services;
using Snowflake.Input.Controller;
using Snowflake.Input.Tests;
using Snowflake.Input.Device;

namespace Snowflake.Configuration.Serialization
{
    public class InputConfigurationSerializationTests
    {
        [Fact]
        public void InputTemplateToAbstractConfigurationNodeIniSerializer_Test()
        {
            var mapcol = new ControllerElementMappings("Keyboard",
                            "TEST_CONTROLLER",
                            InputDriverType.Keyboard,
                            IDeviceEnumerator.VirtualVendorID,
                            new XInputDeviceInstance(0).DefaultLayout);
            IDeviceInputMapping mapping = new TestInputMapping();
            var input =
             new InputTemplate<IRetroArchInput>(mapcol).Template;


            var fs = new PhysicalFileSystem();
            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            var dir = new FS.Directory("test", pfs, pfs.GetDirectoryEntry("/"));

            var context = new ConfigurationTraversalContext();

            var list = context.TraverseInputTemplate(input, mapping, 0);

            var iniSerializer = new SimpleIniConfigurationSerializer();
            string outputIni = iniSerializer.Transform(list);
            var parser = new IniDataParser();
            var data = parser.Parse(outputIni);
            Assert.NotEmpty(data.Sections);
        }

        [Fact]
        public void InputTemplateToAbstractConfigurationNodeCfgSerializer_Test()
        {
            var mapcol = new ControllerElementMappings("Keyboard",
                           "TEST_CONTROLLER",
                           InputDriverType.Keyboard,
                           IDeviceEnumerator.VirtualVendorID,
                           new XInputDeviceInstance(0).DefaultLayout);
            IDeviceInputMapping mapping = new TestInputMapping();
            var input =
             new InputTemplate<IRetroArchInput>(mapcol).Template;


            var fs = new PhysicalFileSystem();
            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            var dir = new FS.Directory("test", pfs, pfs.GetDirectoryEntry("/"));

            var context = new ConfigurationTraversalContext();

            var list = context.TraverseInputTemplate(input, mapping, 0);


            var cfgSerializer = new SimpleCfgConfigurationSerializer();
            string outputCfg = cfgSerializer.Transform(list);
            Assert.NotEqual(string.Empty, outputCfg);
            // todo: test cfg parse
        }

        [Fact]
        public void InputTemplateToAbstractConfigurationNodeXmlSerializer_Test()
        {
            var mapcol = new ControllerElementMappings("Keyboard",
                           "TEST_CONTROLLER",
                           InputDriverType.Keyboard,
                           IDeviceEnumerator.VirtualVendorID,
                           new XInputDeviceInstance(0).DefaultLayout);
            IDeviceInputMapping mapping = new TestInputMapping();
            var input =
             new InputTemplate<IRetroArchInput>(mapcol).Template;


            var fs = new PhysicalFileSystem();
            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            var dir = new FS.Directory("test", pfs, pfs.GetDirectoryEntry("/"));

            var context = new ConfigurationTraversalContext();

            var list = context.TraverseInputTemplate(input, mapping, 0);

            var xmlSerializer = new SimpleXmlConfigurationSerializer("Config");
           
            string outputXml = xmlSerializer.Transform(list);
            XDocument doc = XDocument.Parse(outputXml);
            Assert.NotEmpty(doc.Nodes());
        }

        [Fact]
        public void InputTemplateToAbstractConfigurationNodeJsonSerializer_Test()
        {
            var mapcol = new ControllerElementMappings("Keyboard",
                          "TEST_CONTROLLER",
                          InputDriverType.Keyboard,
                          IDeviceEnumerator.VirtualVendorID,
                          new XInputDeviceInstance(0).DefaultLayout);
            IDeviceInputMapping mapping = new TestInputMapping();
            var input =
             new InputTemplate<IRetroArchInput>(mapcol).Template;


            var fs = new PhysicalFileSystem();
            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            var dir = new FS.Directory("test", pfs, pfs.GetDirectoryEntry("/"));

            var context = new ConfigurationTraversalContext();

            var list = context.TraverseInputTemplate(input, mapping, 0);


            var jsonSerializer = new SimpleJsonConfigurationSerializer();
            string outputJson = jsonSerializer.Transform(list);
            var jtoken = JToken.Parse(outputJson);
            Assert.True(jtoken.HasValues);
        }
    }
}
