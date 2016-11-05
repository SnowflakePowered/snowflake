using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Configuration.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SerializableSectionAttribute : Attribute
    {
        public const string FlagsOutputPath = "#flags";

        public string Destination { get; }


        public SerializableSectionAttribute(string destination)
        {
            this.Destination = destination;
        }
    }
}
