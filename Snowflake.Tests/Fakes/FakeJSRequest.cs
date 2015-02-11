using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Tests.Fakes
{
    internal class FakeJSRequest : Snowflake.Ajax.IJSRequest
    {
        public string GetParameter(string paramKey)
        {
            throw new NotImplementedException();
        }

        public string MethodName
        {
            get { throw new NotImplementedException(); }
        }

        public IDictionary<string, string> MethodParameters
        {
            get { throw new NotImplementedException(); }
        }

        public string NameSpace
        {
            get { throw new NotImplementedException(); }
        }
    }
}
