using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Scraping.Extensibility
{
    /// <summary>
    /// Specifies a directive on a <see cref="IScraper"/> that determines
    /// when and what information it must have access to before it is run.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class DirectiveAttribute : Attribute, IScraperDirective
    {
        /// <summary>
        /// Gets the target of the directive. All seeds of this type are
        /// examined when evaluating the directive.
        /// </summary>
        public AttachTarget Target { get; }

        /// <summary>
        /// Gets the directive action, whether or not this directive means to run
        /// the <see cref="IScraper"/> if the specified child type is available on
        /// the <see cref="Target"/>, or whether not exclude and not run if the specified
        /// child type exists on the target.
        /// </summary>
        public Directive Directive { get; }

        /// <summary>
        /// Gets the child type to evaluate this directive on
        /// </summary>
        public string Type { get; }

        public DirectiveAttribute(AttachTarget target, Directive directive, string type)
        {
            this.Target = target;
            this.Directive = directive;
            this.Type = type;
        }
    }
}
