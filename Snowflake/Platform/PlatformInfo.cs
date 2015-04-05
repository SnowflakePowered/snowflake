using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Information.MediaStore;
using Snowflake.Information;
using Snowflake.Controller;
using System.Collections.ObjectModel;
using Snowflake.Extensions;
using Newtonsoft.Json.Linq;

namespace Snowflake.Platform
{
    public class PlatformInfo : Info, IPlatformInfo
    {
        public PlatformInfo(string platformId, string name, IDictionary<string, string> metadata, IList<string> fileExtensions, IPlatformDefaults platformDefaults, IList<string> controllers, int maximumInputs, IPlatformControllerPorts controllerPorts): base(platformId, name, metadata)
        {
            this.FileExtensions = fileExtensions;
            this.Defaults = platformDefaults;
            this.Controllers = controllers;
            this.ControllerPorts = controllerPorts;
            this.MaximumInputs = maximumInputs;
        }
       
        public IList<string> FileExtensions { get; private set; }
        public IPlatformDefaults Defaults { get; set; }
        public IList<string> Controllers { get; private set; }
        public IPlatformControllerPorts ControllerPorts { get; private set; }
        public int MaximumInputs { get; private set; }
        public static IPlatformInfo FromJsonProtoTemplate(IDictionary<string, dynamic> jsonDictionary)
        {
            IPlatformDefaults platformDefaults = jsonDictionary["Defaults"].ToObject<PlatformDefaults>();

            return new PlatformInfo(
                    jsonDictionary["PlatformID"],
                    jsonDictionary["Name"],
                    jsonDictionary["Metadata"].ToObject<Dictionary<string, string>>(),
                    jsonDictionary["FileExtensions"].ToObject<List<string>>(),
                    platformDefaults,
                    jsonDictionary["Controllers"].ToObject<List<string>>(),
                    (int)jsonDictionary["MaximumInputs"] ,
                    PlatformControllerPorts.ParseControllerPorts(jsonDictionary["ControllerPorts"].ToObject<Dictionary<string, string>>())
                );
        }

    }
}
