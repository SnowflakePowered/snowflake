using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
namespace Snowflake.Plugin
{
    [InheritedExport(typeof(IGeneralPlugin))]
    public interface IGeneralPlugin : IBasePlugin
    {
    }
}
