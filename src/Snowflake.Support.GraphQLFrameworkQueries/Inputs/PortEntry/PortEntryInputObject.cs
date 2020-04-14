using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Input.Device;

namespace Snowflake.Support.GraphQLFrameworkQueries.Inputs.PortEntry
{
    public class PortEntryInputObject
    {
        public Guid InstanceGuid { get; set; }
        public string ControllerId { get; set; }
        public string MappingsProfile { get; set; }
        public InputDriver InstanceDriver { get; set; }
    }
}
