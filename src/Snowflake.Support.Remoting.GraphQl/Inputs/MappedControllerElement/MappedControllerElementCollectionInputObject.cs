using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.Remoting.GraphQl.Inputs.MappedControllerElement
{
    public class MappedControllerElementCollectionInputObject
    {
        public string DeviceId { get; set; }
        public string ControllerId { get; set; }
        public string ProfileName { get; set; }
        public IList<MappedControllerElementInputObject> Mappings { get; set; } 
    }
}
