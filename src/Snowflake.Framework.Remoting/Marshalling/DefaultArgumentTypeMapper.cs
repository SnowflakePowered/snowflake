using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Framework.Remoting.Marshalling
{
    internal class DefaultArgumentTypeMapper : ArgumentTypeMapper
    {
        public DefaultArgumentTypeMapper()
        {
            this.Add<byte>(s => Byte.Parse(s));
            this.Add<sbyte>(s => SByte.Parse(s));
            this.Add<char>(s => Char.Parse(s));
            this.Add<short>(s => Int16.Parse(s));
            this.Add<int>(s => Int32.Parse(s));
            this.Add<long>(s => Int64.Parse(s));
            this.Add<ushort>(s => UInt16.Parse(s));
            this.Add<uint>(s => UInt32.Parse(s));
            this.Add<ulong>(s => UInt64.Parse(s));
            this.Add<Guid>(s => Guid.Parse(s));
            this.Add<double>(s => Double.Parse(s));
            this.Add<float>(s => Single.Parse(s));
            this.Add<decimal>(s => Decimal.Parse(s));
            this.Add<bool>(s => Boolean.Parse(s));
        }
    }
}
