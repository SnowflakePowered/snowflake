using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Tests.Fakes
{
    internal class FakeControllerProfile : Snowflake.Controller.IControllerProfile
    {

        public string ControllerID
        {
            get { throw new NotImplementedException(); }
        }

        public IReadOnlyDictionary<string, string> InputConfiguration
        {
            get { throw new NotImplementedException(); }
        }

        public Controller.ControllerProfileType ProfileType
        {
            get { throw new NotImplementedException(); }
        }

        public IDictionary<string, dynamic> ToSerializable()
        {
            throw new NotImplementedException();
        }
    }
}
