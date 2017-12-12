using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Scraping.Extensibility
{
    /// <summary>
    /// A traverser traverses the resultant <see cref="ISeedRootContext"/>,
    /// or rather sub-trees relative to a given seed in the context,
    /// and produces a list of <see cref="T"/> given the information available in the seed tree.
    /// </summary>
    /// <typeparam name="T">The type of record or object this traverser produces.</typeparam>
    public interface ITraverser<T>
    {
        /// <summary>
        /// Traverses the seed tree to yield objects of type <see cref="T"/>
        /// <para>
        /// While the full seed root context is given, it is best practice for traversers to
        /// treat the given relative root as the root of the entire seed tree rather than the true
        /// root of the context.
        /// </para>
        /// </summary>
        /// <param name="relativeRoot">The seed to begin traversing from.</param>
        /// <param name="context">The seed context within where the seeds can be traversed.</param>
        /// <returns>Objects based on values found in the tree.</returns>
        IEnumerable<T> Traverse(ISeed relativeRoot, ISeedRootContext context);
    }
}
