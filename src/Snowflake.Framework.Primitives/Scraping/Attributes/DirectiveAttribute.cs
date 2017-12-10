using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Scraping.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class DirectiveAttribute : Attribute, IScraperDirective
    {
        public AttachTarget Target { get; }
        public Directive Directive { get; }
        public string Type { get; }

        public DirectiveAttribute(AttachTarget target, Directive directive, string type)
        {
            this.Target = target;
            this.Directive = directive;
            this.Type = type;
        }
    }
}
