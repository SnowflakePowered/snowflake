using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Extensibility;

namespace Snowflake.Scraping.Extensibility
{
    /// <summary>
    /// A <see cref="ICuller"/> filters seeds that remain in consideration
    /// when the <see cref="ISeedRootContext"/> is traversed to produce a result.
    /// </summary>
    public interface ICuller : IPlugin
    {
        /// <summary>
        /// Gets the type of <see cref="ISeed"/> that this culler examines.
        /// </summary>
        string TargetType { get; }

        /// <summary>
        /// Determines the seeds that REMAIN for traversal.
        /// </summary>
        /// <param name="seedsToFilter">The seeds of the specified type in the current job.</param>
        /// <param name="context">The root context of the current job.</param>
        /// <returns>A list of seeds NOT to cull.</returns>
        IEnumerable<ISeed> Filter(IEnumerable<ISeed> seedsToFilter, ISeedRootContext context);
    }
}
