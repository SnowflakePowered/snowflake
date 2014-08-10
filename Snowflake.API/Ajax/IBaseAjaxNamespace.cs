using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Plugin;

namespace Snowflake.Ajax
{
    [InheritedExport(typeof(IBaseAjaxNamespace))]
    public interface IBaseAjaxNamespace : IPlugin
    {
        IDictionary<string, Func<JSRequest, JSResponse>> JavascriptMethods { get; }
    }
}
