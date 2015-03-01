
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Tests.Fakes
{
    internal class FakePlatformInfo : Snowflake.Platform.IPlatformInfo
    {
        public Platform.IPlatformDefaults Defaults
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IList<string> FileExtensions
        {
            get { throw new NotImplementedException(); }
        }

        public int MaximumInputs
        {
            get { throw new NotImplementedException(); }
        }

        public IList<string> Controllers
        {
            get { throw new NotImplementedException(); }
        }

        public Platform.IPlatformControllerPorts ControllerPorts
        {
            get { throw new NotImplementedException(); }
        }

        public string PlatformID
        {
            get { throw new NotImplementedException(); }
        }

        public string Name
        {
            get { throw new NotImplementedException(); }
        }

        public IDictionary<string, string> Metadata
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public Information.MediaStore.IMediaStore MediaStore
        {
            get { throw new NotImplementedException(); }
        }
    }
}
