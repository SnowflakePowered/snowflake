using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Filesystem;
using Snowflake.Installation;
using Snowflake.Installation.Extensibility;
using Snowflake.Model.Game;
using Snowflake.Model.Records;

namespace Snowflake.Scraping.Extensibility
{
    /// <summary>
    /// A traverser traverses the resultant <see cref="ISeedRootContext"/>,
    /// or rather sub-trees relative to a given seed in the context, and applies side effects
    /// according to the final seed tree, producing a list of <see cref="T"/> given the 
    /// information available in the seed tree.
    /// </summary>
    /// <typeparam name="TProducts">The type of record or object this traverser produces.</typeparam>
    /// <typeparam name="TEffectTarget">The type of object this traverser is allowed to mutate.</typeparam>
    public interface ITraverser<TProducts, TEffectTarget>
    {
        /// <summary>
        /// Traverses the seed tree to yield objects of type <see cref="T"/>
        /// <para>
        /// While the full seed root context is given, it is best practice for traversers to
        /// treat the given relative root as the root of the entire seed tree rather than the true
        /// root of the context.
        /// </para>
        /// </summary>
        /// <param name="sideEffectContext">The mutable object onto which side effects may be applied</param>
        /// <param name="relativeRoot">The seed to begin traversing from.</param>
        /// <param name="context">The seed context within where the seeds can be traversed.</param>
        /// <returns>Objects based on values found in the tree.</returns>
        IAsyncEnumerable<TProducts> Traverse(TEffectTarget sideEffectContext, ISeed relativeRoot, ISeedRootContext context);
    }

    /// <summary>
    /// A GameMetadataTraverser is a specialized traverser that yields metadata for a game given
    /// a seed root context.
    /// </summary>
    public interface IGameMetadataTraverser : ITraverser<IRecordMetadata, IGame> { }

    /// <summary>
    /// A FileInstallationTraverser is a specialized traverser that installs additional files to a game,
    /// such as images and videos, yielding them much in the same way as <see cref="IGameInstaller"/>.
    /// </summary>
    public interface IFileInstallationTraverser : ITraverser<TaskResult<IFile>, IGame> { }
}
