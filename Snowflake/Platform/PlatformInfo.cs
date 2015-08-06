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
        public PlatformInfo(string platformId, string name, IDictionary<string, string> metadata, IList<string> fileExtensions, IList<string> controllers, int maximumInputs, IPlatformControllerPorts controllerPorts): base(platformId, name, metadata)
        {
            this.FileExtensions = fileExtensions;
            this.Controllers = controllers;
            this.ControllerPorts = controllerPorts;
            this.MaximumInputs = maximumInputs;
        }
       
        public IList<string> FileExtensions { get; }
        public IList<string> Controllers { get; }
        public IPlatformControllerPorts ControllerPorts { get; }
        public int MaximumInputs { get; }
        public static IPlatformInfo FromJsonProtoTemplate(IDictionary<string, dynamic> jsonDictionary)
        {
            return new PlatformInfo(
                    jsonDictionary["PlatformID"],
                    jsonDictionary["Name"],
                    jsonDictionary["Metadata"].ToObject<Dictionary<string, string>>(),
                    jsonDictionary["FileExtensions"].ToObject<List<string>>(),
                    jsonDictionary["Controllers"].ToObject<List<string>>(),
                    (int)jsonDictionary["MaximumInputs"] ,
                    PlatformControllerPorts.ParseControllerPorts(jsonDictionary["ControllerPorts"].ToObject<Dictionary<string, string>>())
                );
        }

    }
}
