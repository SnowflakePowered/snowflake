using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Scraping
{
    public interface ITraverser<T>
    {
        IEnumerable<T> Traverse(ISeed relativeRoot, ISeedRootContext context);
    }
}
