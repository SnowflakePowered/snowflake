using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shiragame.Builder
{
    class Program
    {
        static void Main(string[] args)
        {
            var openvgdb = new OpenVgdb("ovgdb.db");
            var records = openvgdb.GetEverything().ToList();
        }
    }
}
