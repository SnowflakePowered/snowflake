using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Scraping
{
    public interface ICuller
    {
        string TargetType { get; }
        IEnumerable<ISeed> Filter(IEnumerable<ISeed> seedsToTrim);
    }
}
