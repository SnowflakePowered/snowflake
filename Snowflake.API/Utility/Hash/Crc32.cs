//Forked from https://github.com/damieng/DamienGKit/blob/master/CSharp/DamienG.Library/Security/Cryptography/Crc32.cs

// Copyright (c) Damien Guard.  All rights reserved.
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// Originally published at http://damieng.com/blog/2006/08/08/calculating_crc32_in_c_and_net

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.IO;

namespace Snowflake.Utility.Hash
{
    /// <summary>
    /// Implements a 32-bit CRC hash algorithm compatible with Zip etc.
    /// </summary>
    /// <remarks>
    /// Crc32 should only be used for backward compatibility with older file formats
    /// and algorithms. It is not secure enough for new applications.
    /// If you need to call multiple times for the same data either use the HashAlgorithm
    /// interface or remember that the result of one Compute call needs to be ~ (XOR) before
    /// being passed in as the seed for the next Compute call.
    /// </remarks>
    internal sealed class Crc32 : HashAlgorithm
    {
        public const UInt32 DefaultPolynomial = 0xedb88320u;
        public const UInt32 DefaultSeed = 0xffffffffu;

        private static UInt32[] defaultTable;

        private readonly UInt32 seed;
        private readonly UInt32[] table;
        private UInt32 hash;

        public Crc32()
            : this(Crc32.DefaultPolynomial, Crc32.DefaultSeed)
        {
        }

        public Crc32(UInt32 polynomial, UInt32 seed)
        {
            this.table = Crc32.InitializeTable(polynomial);
            this.seed = this.hash = seed;
        }

        public override void Initialize()
        {
            this.hash = this.seed;
        }

        protected override void HashCore(byte[] buffer, int start, int length)
        {
            this.hash = Crc32.CalculateHash(this.table, this.hash, buffer, start, length);
        }

        protected override byte[] HashFinal()
        {
            var hashBuffer = Crc32.UInt32ToBigEndianBytes(~this.hash);
            this.HashValue = hashBuffer;
            return hashBuffer;
        }

        public override int HashSize { get { return 32; } }

        public static UInt32 Compute(byte[] buffer)
        {
            return Crc32.Compute(Crc32.DefaultSeed, buffer);
        }

        public static UInt32 Compute(UInt32 seed, byte[] buffer)
        {
            return Crc32.Compute(Crc32.DefaultPolynomial, seed, buffer);
        }

        public static UInt32 Compute(UInt32 polynomial, UInt32 seed, byte[] buffer)
        {
            return ~Crc32.CalculateHash(Crc32.InitializeTable(polynomial), seed, buffer, 0, buffer.Length);
        }

        private static UInt32[] InitializeTable(UInt32 polynomial)
        {
            if (polynomial == Crc32.DefaultPolynomial && Crc32.defaultTable != null)
                return Crc32.defaultTable;

            var createTable = new UInt32[256];
            for (var i = 0; i < 256; i++)
            {
                var entry = (UInt32)i;
                for (var j = 0; j < 8; j++)
                    if ((entry & 1) == 1)
                        entry = (entry >> 1) ^ polynomial;
                    else
                        entry = entry >> 1;
                createTable[i] = entry;
            }

            if (polynomial == Crc32.DefaultPolynomial)
                Crc32.defaultTable = createTable;

            return createTable;
        }

        private static UInt32 CalculateHash(UInt32[] table, UInt32 seed, IList<byte> buffer, int start, int size)
        {
            var crc = seed;
            for (var i = start; i < size - start; i++)
                crc = (crc >> 8) ^ table[buffer[i] ^ crc & 0xff];
            return crc;
        }

        private static byte[] UInt32ToBigEndianBytes(UInt32 uint32)
        {
            var result = BitConverter.GetBytes(uint32);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(result);

            return result;
        }

        public static string GetHash(FileStream file)
        {
            using (var crc32 = new Crc32())
                return BitConverter.ToString(crc32.ComputeHash(file)).Replace("-", String.Empty).ToLowerInvariant();

        }
    }
}