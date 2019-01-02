namespace Snowflake.Scraping.Extensibility
{
    /// <summary>
    /// Specifies the seed to which the results are attached or applied to.
    /// In the case of <see cref="DirectiveAttribute"/>, specifies the
    /// seed with which the directive applies to.
    /// </summary>
    public enum AttachTarget
    {
        /// <summary>
        /// Attaches results to the target, or otherwise directs focus to the
        /// specified target seed.
        /// In other words, results will be children of the target seed node.
        /// </summary>
        Target,

        /// <summary>
        /// Attaches results to the parent of the target, or otherwise
        /// directs focus to the parent of the specified target seed.
        /// In other words, results will be siblings of the target seed node.
        /// If the target has no parent, results will be attached to the root seed.
        /// </summary>
        TargetParent,

        /// <summary>
        /// Attaches results to the root seed, or otherwise
        /// directs focus to the root seed.
        /// In other words, results will be children of the root seed node.
        /// </summary>
        Root,
    }
}
