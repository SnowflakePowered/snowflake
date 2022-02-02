using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Support.Orchestration.Overlay.Runtime.Windows.Hooks
{
    internal static class GuidHelpers
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe Guid* RiidOf(Guid @in)
        {
            return &@in;
        }
    }
}
