using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.API.Interface
{
    public interface IScraper : IPlugin
    {
        public string ScraperSource { get; private set; }
        public Dictionary<string, string> ScraperMap { get; private set; }
    }
}
