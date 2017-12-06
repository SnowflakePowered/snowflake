using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Scraping
{
    public interface ITrimmer
    {
        ISeed Trim(IEnumerable<ISeed> seedsToTrim);
    }
}
