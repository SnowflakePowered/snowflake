using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Scraping
{
    public interface ICuller
    {
        IEnumerable<ISeed> Cull(IEnumerable<ISeed> seedsToTrim);
    }
}
