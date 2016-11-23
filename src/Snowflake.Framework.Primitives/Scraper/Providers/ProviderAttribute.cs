using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Records.Metadata;

namespace Snowflake.Scraper.Providers
{
    /// <summary>
    /// Marks a method as a provider function.
    /// Must also be marked as <see cref="RequiredMetadataAttribute"/>,
    /// and is expected to have signature that accepts a single <see cref="IMetadataCollection"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ProviderAttribute : Attribute
    {
    }
}
