/**
* The MIT License (MIT)
*
* Copyright (c) 2016 force
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in all
* copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Force.Crc32;
using Xunit;
using E = Force.Crc32.Crc32Algorithm;

namespace Snowflake.Utility.Crc32
{
    public class ForceCrc32ImplementationFacts
    {
        public class ImplementationFact
        {
            [Theory]
            [InlineData("Hello", 3)]
            [InlineData("Nazdar", 0)]
            [InlineData("Ahoj", 1)]
            [InlineData("Very long text.Very long text.Very long text.Very long text.Very long text.Very long text.Very long text", 0)]
            [InlineData("Very long text.Very long text.Very long text.Very long text.Very long text.Very long text.Very long text", 3)]
            public void ResultConsistency(string text, int offset)
            {
                var bytes = Encoding.ASCII.GetBytes(text);

                var crc1 = E.Compute(bytes.Skip(offset).ToArray());
                var crc2 = Crc32Algorithm.Append(0, bytes, offset, bytes.Length - offset);
                Assert.Equal(crc1, crc2);
            }

            [Fact]
            public void ResultConsistency2()
            {
                Assert.Equal<uint>(2768625435, Crc32Algorithm.Compute(new byte[] { 1 }));
                Assert.Equal<uint>(622876539, Crc32Algorithm.Compute(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }));
            }

            [Fact]
            public void ResultConsistencyAsHashAlgorithm()
            {
                var bytes = new byte[30000];
                new Random().NextBytes(bytes);
                var e = new E();
                var c = new Crc32Algorithm();
                var crc1 = BitConverter.ToInt32(e.ComputeHash(bytes), 0);
                var crc2 = BitConverter.ToInt32(c.ComputeHash(bytes), 0);
                Console.WriteLine(crc1.ToString("X8"));
                Console.WriteLine(crc2.ToString("X8"));
                Assert.Equal(crc1, crc2);
            }

            [Fact]
            public void PartIsWhole()
            {
                var bytes = new byte[30000];
                new Random().NextBytes(bytes);
                var r1 = Crc32Algorithm.Append(0, bytes, 0, 15000);
                var r2 = Crc32Algorithm.Append(r1, bytes, 15000, 15000);
                var r3 = Crc32Algorithm.Append(0, bytes, 0, 30000);
                Assert.Equal(r2, r3);
            }

            [Fact]
            public void Result_Is_BigEndian()
            {
                var bytes = new byte[30000];
                new Random().NextBytes(bytes);
                var crc1 = Crc32Algorithm.Append(0, bytes, 0, bytes.Length);
                var crc2Bytes = new Crc32Algorithm().ComputeHash(bytes);
                if (BitConverter.IsLittleEndian)
                {
                    crc2Bytes = crc2Bytes.Reverse().ToArray();
                }

                var crc2 = BitConverter.ToUInt32(crc2Bytes, 0);
                Assert.Equal(crc1, crc2);
            }
        }
    }
}
