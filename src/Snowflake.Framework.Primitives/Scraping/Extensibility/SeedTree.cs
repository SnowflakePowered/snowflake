using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Scraping.Extensibility
{
    /// <summary>
    /// Used when building a seed tree, represents a <see cref="SeedContent"/> with
    /// context-free parent/child relationships. This should never be used directly
    /// outside of the return value of an <see cref="IScraper"/>. There is
    /// no way to instantiate this class besides implicitly converting from
    /// an instance of <see cref="SeedContent"/>. <see cref="SeedTree"/> does not
    /// implement <see cref="IEquatable{SeedTree}"/>, and thus is not equatable.
    /// </summary>
    public struct SeedTree
    {
        /// <summary>
        /// Implicitly coerces a <see cref="SeedTree"/> into
        /// a single <see cref="SeedContent"/> without children.
        /// </summary>
        /// <param name="seedTree">The <see cref="SeedTree"/> to coerce.</param>
        public static implicit operator SeedContent(SeedTree seedTree)
        {
            return seedTree.Content;
        }

        /// <summary>
        /// Implicitly coerces a <see cref="SeedContent"/> into
        /// a single <see cref="SeedTree"/> without children.
        /// </summary>
        /// <param name="seedContent">The <see cref="SeedContent"/> to coerce.</param>
        public static implicit operator SeedTree
            (SeedContent seedContent)
        {
            return new SeedTree(seedContent);
        }

        /// <summary>
        /// Implicitly coerces a <see cref="SeedContent"/> into
        /// a single <see cref="SeedTree"/> without children.
        /// </summary>
        /// <param name="seedContent">The <see cref="SeedContent"/> to coerce.</param>
        public static implicit operator SeedTree
            ((string type, string value) seedContent)
        {
            return new SeedTree(seedContent);
        }

        /// <summary>
        /// Implicitly coerces a <see cref="SeedTree"/> into
        /// a single <see cref="SeedContent"/> without children.
        /// </summary>
        /// <param name="seedTree">The <see cref="SeedTree"/> to coerce.</param>
        public static implicit operator (string type, string value)(SeedTree seedTree)
        {
            return seedTree.Content;
        }

        /// <summary>
        /// Implicitly coerces a <see cref="ValueTuple{SeedContent, IEnumerable{SeedTree}}"/>
        /// into a <see cref="SeedTree"/>
        /// </summary>
        /// <param name="seedTree">The <see cref="ValueTuple{SeedContent, IEnumerable{SeedTree}}"/> to convert.</param>
        public static implicit operator ((string type, string value) content, IEnumerable<SeedTree> children)
            (SeedTree seedTree)
        {
            return (seedTree.Content, seedTree.Children);
        }

        /// <summary>
        /// Implicitly coerces a <see cref="SeedTree"/>
        /// into a <see cref="ValueTuple{SeedContent, IEnumerable{SeedTree}}"/>
        /// </summary>
        /// <param name="seedTree">The <see cref="SeedTree"/> to convert.</param>
        public static implicit operator (SeedContent content, IEnumerable<SeedTree> children)
            (SeedTree seedTree)
        {
            return (seedTree.Content, seedTree.Children);
        }

        /// <summary>
        /// Implicitly coerces a <see cref="ValueTuple{SeedContent, IEnumerable{SeedTree}}"/>
        /// into a <see cref="SeedTree"/>
        /// </summary>
        /// <param name="contentTuple">The <see cref="ValueTuple{SeedContent, IEnumerable{SeedTree}}"/> to convert.</param>
        public static implicit operator SeedTree((SeedContent content, IEnumerable<SeedTree> children)
            contentTuple)
        {
            return new SeedTree(contentTuple.content, contentTuple.children);
        }

        /// <summary>
        /// Implicit coerces a <see cref="ValueTuple{String, String, IEnumerable{SeedTree}}"/> into
        /// a <see cref="SeedTree"/>
        /// </summary>
        /// <param name="contentTuple">The <see cref="ValueTuple{String, String, IEnumerable{SeedTree}}"/> to coerce.</param>
        public static implicit operator SeedTree((string type, string value, IEnumerable<SeedTree> children)
            contentTuple)
        {
            return new SeedTree((contentTuple.type, contentTuple.value), contentTuple.children);
        }

        /// <summary>
        /// Gets the <see cref="SeedContent"/> associated with this tree.
        /// </summary>
        public SeedContent Content { get; }

        /// <summary>
        /// Gets the children of this <see cref="SeedTree"/>
        /// </summary>
        public IEnumerable<SeedTree> Children { get; }

        private SeedTree(SeedContent content, params SeedTree[] children)
        {
            this.Content = content;
            this.Children = children;
        }

        private SeedTree(SeedContent content, IEnumerable<SeedTree> children)
        {
            this.Content = content;
            this.Children = children;
        }
    }
}
