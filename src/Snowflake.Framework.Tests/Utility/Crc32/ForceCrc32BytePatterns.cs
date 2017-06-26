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

using Force.Crc32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using System.Data.HashFunction;

namespace Snowflake.Utility.Crc32.Tests
{
    public class ForceCrc32BytePatterns
    {
        public class BytePatternsFact
        {
            [Fact]
            public void Crc32ForEmptySequenseIs0()
            {
                var actual = Crc32Algorithm.Compute(new byte[0]);
                Assert.Equal<uint>(0, actual);
            }

            // Pattern: 
            // xx
            // xx xx
            // xx xx xx
            // ...
            [Fact]
            public void RepeatedBytePatternFact()
            {
                foreach (var x in Enumerable.Range(0, 256))
                {
                    foreach (int len in Enumerable.Range(1, 32))
                    {
                        var data = Enumerable.Repeat((byte)x, len).ToArray();
                        FactByteSequence(data);
                    }
                }
            }

            // Pattern:
            // xx
            // xx 00
            // 00 xx
            // xx 00 00
            // 00 xx 00
            // 00 00 xx
            // ...

            // xx
            // xx FF
            // FF xx
            // xx FF FF
            // FF xx FF
            // FF FF xx
            // ...
            [Theory]
            [InlineData(0x00)]
            [InlineData(0xFF)]
            public void SlidingBytePatternFact(byte fillValue)
            {
                foreach (int len in Enumerable.Range(1, 32))
                {
                    var data = Enumerable.Repeat(fillValue, len).ToArray();

                    foreach (var i in Enumerable.Range(0, len))
                    {
                        foreach (var x in Enumerable.Range(0, 256))
                        {
                            data[i] = (byte)x;
                            FactByteSequence(data);
                        }

                        data[i] = fillValue;
                    }
                }
            }

            private void FactByteSequence(byte[] data)
            {
                var actual = Crc32Algorithm.Compute(data);
                var expected = BitConverter.ToUInt32(new CRC().ComputeHash(data), 0);

                if (expected != actual)
                {
                    var message = string.Format(
                        "Fact failed for {0}\nExpected: {1:x8}\nBut was: {2:x8}",
                        BitConverter.ToString(data),
                        expected,
                        actual);
                    Assert.True(false, message);
                }
            }
        }
    }
}
