using GraphQL.Types;
using Snowflake.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.Remoting.GraphQl.Types.Configuration
{
    public class SelectionOptionDescriptorGraphType : ObjectGraphType<ISelectionOptionDescriptor>
    {
        public SelectionOptionDescriptorGraphType()
        {
            Field(p => p.DisplayName).Description("The display name of the selection option.");
            Field(p => p.Private).Description("Whether or not to show this option to the user.");
            Field(p => p.SerializeAs).Description("What this option is serialized to in the emulation configuration.");
            Field(p => p.NumericValue).Description("The numeric enumeration value of this selection.");
        }
    }
}
