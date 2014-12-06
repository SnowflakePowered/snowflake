using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Platform.Controller
{
    public class ControllerConfiguration
    {
        public IReadOnlyDictionary<string, string> InputConfiguration { get; set; }
        private IDictionary<string, string> inputConfiguration;


    }
}
