using Snowflake.Model.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Orchestration.SystemFiles
{
    public interface ISystemFile
    {
        PlatformId Platform { get; }
        string Md5Hash { get; }
    }
}
