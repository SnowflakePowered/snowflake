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

namespace Snowflake.Configuration.Serialization
{
    public class InputConfigurationSerializationTests
    {
        [Fact]
        public void InputTemplateToAbstractConfigurationNodeIniSerializer_Test()
        {
            var testmappings = new StoneProvider().Controllers["XBOX_CONTROLLER"];
            var realmapping =
                JsonConvert.DeserializeObject<ControllerLayout>(
                    TestUtilities.GetStringResource("InputMappings.xinput_device.json"));
            var mapcol = ControllerElementMappings.GetDefaultMappings(realmapping, testmappings);
            string _mapping = TestUtilities.GetStringResource("InputMappings.DirectInput.XINPUT_DEVICE.json");
            IInputMapping mapping = JsonConvert.DeserializeObject<InputMapping>(_mapping);
            var input =
             new InputTemplate<IRetroArchInput>(mapcol).Template;


            var fs = new PhysicalFileSystem();
            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            var dir = new FS.Directory("test", pfs, pfs.GetDirectoryEntry("/"));

            var context = new ConfigurationTraversalContext(dir);

            var list = context.TraverseInputTemplate(input, mapping, 0);

            var iniSerializer = new SimpleIniConfigurationSerializer();
            string outputIni = iniSerializer.Serialize(list);
            var parser = new IniDataParser();
            var data = parser.Parse(outputIni);
            Assert.NotEmpty(data.Sections);
        }

        [Fact]
        public void InputTemplateToAbstractConfigurationNodeCfgSerializer_Test()
        {
            var testmappings = new StoneProvider().Controllers["XBOX_CONTROLLER"];
            var realmapping =
                JsonConvert.DeserializeObject<ControllerLayout>(
                    TestUtilities.GetStringResource("InputMappings.xinput_device.json"));
            var mapcol = ControllerElementMappings.GetDefaultMappings(realmapping, testmappings);
            string _mapping = TestUtilities.GetStringResource("InputMappings.DirectInput.XINPUT_DEVICE.json");
            IInputMapping mapping = JsonConvert.DeserializeObject<InputMapping>(_mapping);
            var input =
             new InputTemplate<IRetroArchInput>(mapcol).Template;


            var fs = new PhysicalFileSystem();
            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            var dir = new FS.Directory("test", pfs, pfs.GetDirectoryEntry("/"));

            var context = new ConfigurationTraversalContext(dir);

            var list = context.TraverseInputTemplate(input, mapping, 0);


            var cfgSerializer = new SimpleCfgConfigurationSerializer();
            string outputCfg = cfgSerializer.Serialize(list);
            Assert.NotEqual(string.Empty, outputCfg);
            // todo: test cfg parse
        }

        [Fact]
        public void InputTemplateToAbstractConfigurationNodeXmlSerializer_Test()
        {
            var testmappings = new StoneProvider().Controllers["XBOX_CONTROLLER"];
            var realmapping =
                JsonConvert.DeserializeObject<ControllerLayout>(
                    TestUtilities.GetStringResource("InputMappings.xinput_device.json"));
            var mapcol = ControllerElementMappings.GetDefaultMappings(realmapping, testmappings);
            string _mapping = TestUtilities.GetStringResource("InputMappings.DirectInput.XINPUT_DEVICE.json");
            IInputMapping mapping = JsonConvert.DeserializeObject<InputMapping>(_mapping);
            var input =
             new InputTemplate<IRetroArchInput>(mapcol).Template;


            var fs = new PhysicalFileSystem();
            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            var dir = new FS.Directory("test", pfs, pfs.GetDirectoryEntry("/"));

            var context = new ConfigurationTraversalContext(dir);

            var list = context.TraverseInputTemplate(input, mapping, 0);

            var xmlSerializer = new SimpleXmlConfigurationSerializer("Config");
           
            string outputXml = xmlSerializer.Serialize(list);
            XDocument doc = XDocument.Parse(outputXml);
            Assert.NotEmpty(doc.Nodes());
        }

        [Fact]
        public void InputTemplateToAbstractConfigurationNodeJsonSerializer_Test()
        {
            var testmappings = new StoneProvider().Controllers["XBOX_CONTROLLER"];
            var realmapping =
                JsonConvert.DeserializeObject<ControllerLayout>(
                    TestUtilities.GetStringResource("InputMappings.xinput_device.json"));
            var mapcol = ControllerElementMappings.GetDefaultMappings(realmapping, testmappings);
            string _mapping = TestUtilities.GetStringResource("InputMappings.DirectInput.XINPUT_DEVICE.json");
            IInputMapping mapping = JsonConvert.DeserializeObject<InputMapping>(_mapping);
            var input =
             new InputTemplate<IRetroArchInput>(mapcol).Template;


            var fs = new PhysicalFileSystem();
            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            var dir = new FS.Directory("test", pfs, pfs.GetDirectoryEntry("/"));

            var context = new ConfigurationTraversalContext(dir);

            var list = context.TraverseInputTemplate(input, mapping, 0);


            var jsonSerializer = new SimpleJsonConfigurationSerializer();
            string outputJson = jsonSerializer.Serialize(list);
            var jtoken = JToken.Parse(outputJson);
            Assert.True(jtoken.HasValues);
        }
    }
}
