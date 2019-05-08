using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Extensibility;
using Snowflake.Filesystem;
using Snowflake.Installation;
using Snowflake.Installation.Extensibility;
using Snowflake.Model.Game;
using Snowflake.Model.Records;
using Snowflake.Model.Records.File;
using static System.Runtime.CompilerServices.ConfiguredTaskAwaitable;

namespace Snowflake.Scraping.Extensibility
{
    /// <summary>
    /// A traverser traverses the resultant <see cref="ISeedRootContext"/>,
    /// or rather sub-trees relative to a given seed in the context, and applies side effects
    /// according to the final seed tree, producing a list of <see cref="T"/> given the 
    /// information available in the seed tree.
    /// 
    /// </summary>
    /// <typeparam name="TProducts">The type of record or object this traverser produces.</typeparam>
    /// <typeparam name="TEffectTarget">The type of object this traverser is allowed to mutate.</typeparam>
    public interface ITraverser<TProducts, TEffectTarget>
        : IPlugin
    {
        /// <summary>
        /// Gets the task awaiter for traversers.
        /// Traversers must be awaited context free and thus is always configured to not continue
        /// on captured context.
        /// </summary>
        /// <param name="sideEffectContext">The mutable object onto which side effects may be applied</param>
        /// <param name="relativeRoot">The seed to begin traversing from.</param>
        /// <param name="context">The seed context within where the seeds can be traversed.</param>
        /// <returns>Objects based on values found in the tree.</returns>
        Task<IEnumerable<TProducts>> TraverseAll(TEffectTarget sideEffectContext, ISeed relativeRoot, ISeedRootContext context);
        /// <summary>
        /// Traverses the seed tree to yield objects of type <typeparamref name="TProducts"/>,
        /// which, while being enumerated, may cause side effects to the provided <paramref name="sideEffectContext"/>.
        /// 
        /// This may mean that the provided products are already applied to the <paramref name="sideEffectContext"/>,
        /// and should not be reapplied as enumerated.
        /// <para>
        /// Although the full seed root context is provied, it is best practice for traversers to
        /// traverse relative to the given <paramref name="relativeRoot"/>.
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
    public interface IFileInstallationTraverser : ITraverser<TaskResult<IFileRecord>, IGame> { }
}
