using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Scraping
{
    public struct SeedTree
    {
        public static implicit operator SeedContent(SeedTree seedTree)
        {
            return seedTree.Content;
        }

        public static implicit operator SeedTree
            (SeedContent seedContent)
        {
            return new SeedTree(seedContent);
        }

        public static implicit operator SeedTree
          ((string type, string value) seedContent)
        {
            return new SeedTree(seedContent);
        }

        public static implicit operator (string type, string value) (SeedTree seedTree)
        {
            return seedTree.Content;
        }

        public static implicit operator ((string type, string value) content, IEnumerable<SeedTree> children)
            (SeedTree seedTree)
        {
            return (seedTree.Content, seedTree.Children);
        }

        public static implicit operator (SeedContent content, IEnumerable<SeedTree> children)
            (SeedTree seedTree)
        {
            return (seedTree.Content, seedTree.Children);
        }

        public static implicit operator SeedTree((SeedContent content, IEnumerable<SeedTree> children)
            contentTuple)
        {
            return new SeedTree(contentTuple.content, contentTuple.children);
        }

        public static implicit operator SeedTree((string type, string value, IEnumerable<SeedTree> children)
            contentTuple)
        {
            return new SeedTree((contentTuple.type, contentTuple.value), contentTuple.children);
        }

        public static IEnumerable<SeedTree> With(params SeedTree[] children) => children;

        public SeedContent Content { get; }
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
