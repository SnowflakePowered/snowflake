using Snowflake.Input.Device;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQLFrameworkQueries.Inputs.MappedControllerElement
{
    public class DefaultMappedControllerElementCollectionInputObject
    {
        public Guid InstanceGuid { get; set; }
        public InputDriver InputDriver { get; set; }
        public string ControllerId { get; set; }
        public string ProfileName { get; set; }
    }
}
