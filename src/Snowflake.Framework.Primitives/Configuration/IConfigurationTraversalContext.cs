﻿using System.Collections.Generic;
using Snowflake.Configuration.Input;
using Snowflake.Input.Controller;

namespace Snowflake.Configuration.Serialization
{
    /// <summary>
    /// The context under which a <see cref="IConfigurationCollection"/> or <see cref="IInputTemplate"/>
    /// may be traversed to yield an abstract syntax tree.
    /// </summary>
    public interface IConfigurationTraversalContext
    {
        /// <summary>
        /// Traverses a <see cref="IConfigurationCollection"/> to yield a mapping of
        /// targets to ASTs formed by <see cref="IAbstractConfigurationNode"/>.
        /// 
        /// The special target #null is ignored and never traversed.
        /// </summary>
        /// <param name="collection">
        /// The <see cref="IConfigurationCollection"/> with targets specified to
        /// traverse in order to yield an AST.
        /// </param>
        /// <returns>A mapping of targets to <see cref="IAbstractConfigurationNode{T}"/> that represent the
        /// syntax tree that the target forms.</returns>
        IReadOnlyDictionary<string, IAbstractConfigurationNode<IReadOnlyList<IAbstractConfigurationNode>>> 
            TraverseCollection(IConfigurationCollection collection);

        /// <summary>
        /// Traverses a <see cref="IInputTemplate"/> under the context of an <see cref="IInputMapping"/>
        /// to yield an AST formed by <see cref="IAbstractConfigurationNode"/>.
        /// </summary>
        /// <param name="template">The <see cref="IInputTemplate"/> to traverse in order to yield an AST.</param>
        /// <param name="mapping">
        /// The string mappings for each valid <see cref="ControllerElement"/> in the
        /// <see cref="IInputTemplate"/>, which will be used in accordance to produce the AST.
        /// </param>
        /// <param name="index">
        /// The player index that the input template is intended for. This player index is zero-indexed,
        /// so Player 1 will have an index of 0.
        /// </param>
        /// <param name="indexer">
        /// The special string that will be replaced with the player index as an integer.
        /// By default it is the string {N}, but can be overridden.
        /// </param>
        /// <returns>The syntax tree that the given <see cref="IInputTemplate"/> forms, relative to the provided
        /// <see cref="IInputMapping"/>s.</returns>
        IAbstractConfigurationNode<IReadOnlyList<IAbstractConfigurationNode>> TraverseInputTemplate(IInputTemplate template, 
            IInputMapping mapping, int index, string indexer = "{N}");
    }
}