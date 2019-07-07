using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Configuration.Serialization
{
    /// <summary>
    /// The context with which the serialization for a <see cref="IAbstractConfigurationNode"/> occurs.
    /// See implementations of <see cref="IConfigurationTransformer{TOutput}"/> where TOutput is <see cref="string"/>
    /// or <see cref="byte"/>[].
    /// </summary>
    /// <typeparam name="T">The serialized type the context handles.</typeparam>
    public interface IConfigurationSerializationContext<T>
    {
        /// <summary>
        /// Gets the current resultant state of the serialization. This may be incomplete if serialization is not complete.
        /// </summary>
        T Result { get; }

        /// <summary>
        /// Enters a new scope or block within the serialization. Scoping can be thought of as a stack where
        /// entering pushes a new item to the top of the stack.
        /// </summary>
        /// <param name="scope">The name of the scope.</param>
        /// <returns>The name of the scope entered represented in <typeparamref name="T"/>.</returns>
        T EnterScope(string scope);

        /// <summary>
        /// Exits the current block within the serialization. Scoping can be thought of as a stack where
        /// exiting pops an item off the stack.
        /// </summary>
        /// <returns>The name of the scope exited represented in <typeparamref name="T"/>.</returns>
        T ExitScope();

        /// <summary>
        /// Gets the full representation of current scope, including all parent scopes, as an chronological array where
        /// the earliest entered scope is first.
        /// </summary>
        /// <returns>The full representation of the current scope. If no scopes are entered, then an empty array.</returns>
        T[] GetFullScope();

        /// <summary>
        /// Gets the representation of the current (top-most) scope as <typeparamref name="T"/>.
        /// </summary>
        /// <returns>
        /// The representation of the top-most scope as <typeparamref name="T"/>. If no scopes are present, then the
        /// unit value of <typeparamref name="T"/>; for <see cref="string"/>, this should be the empty string, and
        /// for <see cref="byte"/>[] an empty array or similar, depending on the needs of the serialization.
        /// 
        /// For other types of serialization, choose values that make sense.
        /// </returns>
        T GetCurrentScope();

        /// <summary>
        /// Gets the current number of nested scopes.
        /// </summary>
        int ScopeLevel { get; }

        /// <summary>
        /// Appends new content to the context.
        /// </summary>
        /// <param name="content">The content to append.</param>
        void Append(T content);
    }
}
