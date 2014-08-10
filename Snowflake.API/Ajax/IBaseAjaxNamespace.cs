using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Ajax
{
    [InheritedExport(typeof(IBaseAjaxNamespace))]
    public interface IBaseAjaxNamespace
    {
        IDictionary<string, Func<JSRequest, JSResponse>> JavascriptMethods { get; }
    }
}
