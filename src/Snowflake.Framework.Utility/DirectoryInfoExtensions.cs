using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Snowflake.Utility
{
    public static class DirectoryInfoExtensions
    {
        public static bool IsChildOf(this DirectoryInfo candidateInfo, DirectoryInfo otherInfo)
        {
            var isChild = false;
            try
            {
                while (candidateInfo.Parent != null)
                {
                    if (candidateInfo.Parent.FullName == otherInfo.FullName)
                    {
                        isChild = true;
                        break;
                    }
                    else candidateInfo = candidateInfo.Parent;
                }
            }
            catch
            {
                return false;
            }

            return isChild;
        }
    }
}
