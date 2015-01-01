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
    public interface IBaseAjaxNamespace : IBasePlugin
    {
        IDictionary<string, Func<IJSRequest, IJSResponse>> JavascriptMethods { get; }
    }
}
