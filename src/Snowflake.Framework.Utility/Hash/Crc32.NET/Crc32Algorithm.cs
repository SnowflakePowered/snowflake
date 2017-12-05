﻿using System;
using System.Security.Cryptography;

namespace Force.Crc32
{
    /// <summary>
    /// Implementation of CRC-32.
    /// This class supports several convenient static methods returning the CRC as UInt32.
    /// </summary>
    public class Crc32Algorithm : HashAlgorithm
    {
        private readonly bool isBigEndian;
        private uint currentCrc;

        /// <summary>
        /// Initializes a new instance of the <see cref="Crc32Algorithm"/> class.
        /// </summary>
        /// <param name="isBigEndian">Should return bytes result as big endian or little endian</param>
        // Crc32 by dariogriffo uses big endian, so, we need to be compatible and return big endian as default
        public Crc32Algorithm(bool isBigEndian = true)
        {
            this.isBigEndian = isBigEndian;
#if !NETCORE
            this.HashSizeValue = 32;
#endif
        }

        /// <summary>
        /// Computes CRC-32 from multiple buffers.
        /// Call this method multiple times to chain multiple buffers.
        /// </summary>
        /// <param name="initial">
        /// Initial CRC value for the algorithm. It is zero for the first buffer.
        /// Subsequent buffers should have their initial value set to CRC value returned by previous call to this method.
        /// </param>
        /// <param name="input">Input buffer with data to be checksummed.</param>
        /// <param name="offset">Offset of the input data within the buffer.</param>
        /// <param name="length">Length of the input data in the buffer.</param>
        /// <returns>Accumulated CRC-32 of all buffers processed so far.</returns>
        public static uint Append(uint initial, byte[] input, int offset, int length)
        {
            if (input == null)
            {
                throw new ArgumentNullException();
            }

            if (offset < 0 || length < 0 || offset + length > input.Length)
            {
                throw new ArgumentOutOfRangeException("Selected range is outside the bounds of the input array");
            }

            return AppendInternal(initial, input, offset, length);
        }

        /// <summary>
        /// Computes CRC-32 from multiple buffers.
        /// Call this method multiple times to chain multiple buffers.
        /// </summary>
        /// <param name="initial">
        /// Initial CRC value for the algorithm. It is zero for the first buffer.
        /// Subsequent buffers should have their initial value set to CRC value returned by previous call to this method.
        /// </param>
        /// <param name="input">Input buffer containing data to be checksummed.</param>
        /// <returns>Accumulated CRC-32 of all buffers processed so far.</returns>
        public static uint Append(uint initial, byte[] input)
        {
            if (input == null)
            {
                throw new ArgumentNullException();
            }

            return AppendInternal(initial, input, 0, input.Length);
        }

        /// <summary>
        /// Computes CRC-32 from input buffer.
        /// </summary>
        /// <param name="input">Input buffer with data to be checksummed.</param>
        /// <param name="offset">Offset of the input data within the buffer.</param>
        /// <param name="length">Length of the input data in the buffer.</param>
        /// <returns>CRC-32 of the data in the buffer.</returns>
        public static uint Compute(byte[] input, int offset, int length)
        {
            return Append(0, input, offset, length);
        }

        /// <summary>
        /// Computes CRC-32 from input buffer.
        /// </summary>
        /// <param name="input">Input buffer containing data to be checksummed.</param>
        /// <returns>CRC-32 of the buffer.</returns>
        public static uint Compute(byte[] input)
        {
            return Append(0, input);
        }

        /// <summary>
        /// Computes CRC-32 from input buffer and writes it after end of data (buffer should have 4 bytes reserved space for it). Can be used in conjunction with <see cref="IsValidWithCrcAtEnd(byte[],int,int)"/>
        /// </summary>
        /// <param name="input">Input buffer with data to be checksummed.</param>
        /// <param name="offset">Offset of the input data within the buffer.</param>
        /// <param name="length">Length of the input data in the buffer.</param>
        /// <returns>CRC-32 of the data in the buffer.</returns>
        public static uint ComputeAndWriteToEnd(byte[] input, int offset, int length)
        {
            if (length + 4 > input.Length)
            {
                throw new ArgumentOutOfRangeException("length", "Length of data should be less than array length - 4 bytes of CRC data");
            }

            var crc = Append(0, input, offset, length);
            var r = offset + length;
            input[r] = (byte)crc;
            input[r + 1] = (byte)(crc >> 8);
            input[r + 2] = (byte)(crc >> 16);
            input[r + 3] = (byte)(crc >> 24);
            return crc;
        }

        /// <summary>
        /// Computes CRC-32 from input buffer - 4 bytes and writes it as last 4 bytes of buffer. Can be used in conjunction with <see cref="IsValidWithCrcAtEnd(byte[])"/>
        /// </summary>
        /// <param name="input">Input buffer with data to be checksummed.</param>
        /// <returns>CRC-32 of the data in the buffer.</returns>
        public static uint ComputeAndWriteToEnd(byte[] input)
        {
            if (input.Length < 4)
            {
                throw new ArgumentOutOfRangeException("input", "input array should be 4 bytes at least");
            }

            return ComputeAndWriteToEnd(input, 0, input.Length - 4);
        }

        /// <summary>
        /// Validates correctness of CRC-32 data in source buffer with assumption that CRC-32 data located at end of buffer in reverse bytes order. Can be used in conjunction with <see cref="ComputeAndWriteToEnd(byte[],int,int)"/>
        /// </summary>
        /// <param name="input">Input buffer with data to be checksummed.</param>
        /// <param name="offset">Offset of the input data within the buffer.</param>
        /// <param name="lengthWithCrc">Length of the input data in the buffer with CRC-32 bytes.</param>
        /// <returns>Is checksum valid.</returns>
        public static bool IsValidWithCrcAtEnd(byte[] input, int offset, int lengthWithCrc)
        {
            return Append(0, input, offset, lengthWithCrc) == 0x2144DF1C;
        }

        /// <summary>
        /// Validates correctness of CRC-32 data in source buffer with assumption that CRC-32 data located at end of buffer in reverse bytes order. Can be used in conjunction with <see cref="ComputeAndWriteToEnd(byte[],int,int)"/>
        /// </summary>
        /// <param name="input">Input buffer with data to be checksummed.</param>
        /// <returns>Is checksum valid.</returns>
        public static bool IsValidWithCrcAtEnd(byte[] input)
        {
            if (input.Length < 4)
            {
                throw new ArgumentOutOfRangeException("input", "input array should be 4 bytes at least");
            }

            return Append(0, input, 0, input.Length) == 0x2144DF1C;
        }

        /// <summary>
        /// Resets internal state of the algorithm. Used internally.
        /// </summary>
        public override void Initialize()
        {
            this.currentCrc = 0;
        }

        /// <summary>
        /// Appends CRC-32 from given buffer
        /// </summary>
        protected override void HashCore(byte[] input, int offset, int length)
        {
            this.currentCrc = AppendInternal(this.currentCrc, input, offset, length);
        }

        /// <summary>
        /// Computes CRC-32 from <see cref="HashCore"/>
        /// </summary>
        protected override byte[] HashFinal()
        {
            if (this.isBigEndian)
            {
                return new[] { (byte)(this.currentCrc >> 24), (byte)(this.currentCrc >> 16), (byte)(this.currentCrc >> 8), (byte)this.currentCrc };
            }
            else
            {
                return new[] { (byte)this.currentCrc, (byte)(this.currentCrc >> 8), (byte)(this.currentCrc >> 16), (byte)(this.currentCrc >> 24) };
            }
        }

        private static readonly SafeProxy _proxy = new SafeProxy();

        private static uint AppendInternal(uint initial, byte[] input, int offset, int length)
        {
            if (length > 0)
            {
                return _proxy.Append(initial, input, offset, length);
            }
            else
                return initial;
        }
    }
}