using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Extensibility;

namespace Snowflake.Scraping.Extensibility
{
    public interface ICuller : IPlugin
    {
        string TargetType { get; }
        IEnumerable<ISeed> Filter(IEnumerable<ISeed> seedsToTrim, ISeedRootContext context);
    }
}
