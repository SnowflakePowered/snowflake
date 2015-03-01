using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Game;
namespace Snowflake.Tests.Fakes
{
    internal class FakeGameInfo : IGameInfo
    {
        public string CRC32
        {
            get { throw new NotImplementedException(); }
        }

        public string FileName
        {
            get { throw new NotImplementedException(); }
        }

        public string UUID
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
