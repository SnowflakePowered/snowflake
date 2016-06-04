using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Scraper.Providers
{
    /// <summary>
    /// Marks a method as a provider function
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ProviderAttribute : Attribute
    {
    }
}
