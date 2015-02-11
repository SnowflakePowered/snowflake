using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Tests.Fakes
{
    internal class FakeJSResponse : Snowflake.Ajax.IJSResponse
    {
        public string GetJson()
        {
            throw new NotImplementedException();
        }

        public dynamic Payload
        {
            get { throw new NotImplementedException(); }
        }

        public Ajax.IJSRequest Request
        {
            get { throw new NotImplementedException(); }
        }
    }
}
