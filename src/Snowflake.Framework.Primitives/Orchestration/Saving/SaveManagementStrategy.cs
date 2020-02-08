using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Orchestration.Saving
{
    /// <summary>
    /// Different management strategies used to manage save histories.
    /// </summary>
    public enum SaveManagementStrategy
    {
        /// <summary>
        /// No strategy is used to manage saves. In other words, no saves are persisted.
        /// </summary>
        None,
        /// <summary>
        /// Only one copy of the initial save is kept, which is replaced by new saves. 
        /// Best for extremely large and irregular (random) save files, but loses the 
        /// ability to restore a previous save.
        /// </summary>
        Replace,
        /// <summary>
        /// A full copy of the save contents are kept on the creation of each new save.
        /// Best for small save files that don't take up a lot of space.
        /// </summary>
        Copy,
        /// <summary>
        /// A diff is created from the initial save of the <see cref="ISaveProfile"/>.
        /// If no initial save exists, then the behaviour for the initial save only is
        /// the same as <see cref="Copy"/>.
        /// 
        /// Best for fixed-size, larger save files that are still reasonable in size to calculate
        /// a diff based on the initial save for. Calculating the diff will incur a 
        /// CPU time cost.
        /// </summary>
        Diff,
    }
}
