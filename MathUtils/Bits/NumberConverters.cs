using System.Collections.Generic;
using System.Linq;

namespace MathUtils.Bits
{
    public static class NumberConverters
    {
        public static int DeZero(this int value, int subst)
        {
            return (value == 0) ? subst : value;
        }

        public static int DeZero(this uint value, int subst)
        {
            return (value == 0) ? subst : (int)value;
        }

        public static bool BitValue(this ushort value, int bitPos)
        {
            return (value & 1 << bitPos) > 0;
        }

        public static bool BitValue(this uint value, int bitPos)
        {
            return (value & ((uint)1) << bitPos) > 0;
        }

        public static bool BitValue(this ulong value, int bitPos)
        {
            return (value & ((ulong)1) << bitPos) > 0;
        }

        public static ushort FilpBit(this ushort value, int bitPos)
        {
            return (ushort)(value ^ (1 << bitPos));
        }

        public static uint FilpBit(this uint value, int bitPos)
        {
            return value ^ ((uint)1) << bitPos;
        }

        public static bool[] ToBits(this byte value)
        {
            var retVal = new bool[8];
            retVal[0] = (value & 1) > 0;
            retVal[1] = (value & 2) > 0;
            retVal[2] = (value & 4) > 0;
            retVal[3] = (value & 8) > 0;
            retVal[4] = (value & 16) > 0;
            retVal[5] = (value & 32) > 0;
            retVal[6] = (value & 64) > 0;
            retVal[7] = (value & 128) > 0;
            return retVal;
        }

        public static bool[] ToBits(this ushort value)
        {
            var retVal = new bool[16];
            retVal[0] = (value & 1) > 0;
            retVal[1] = (value & 2) > 0;
            retVal[2] = (value & 4) > 0;
            retVal[3] = (value & 8) > 0;
            retVal[4] = (value & 16) > 0;
            retVal[5] = (value & 32) > 0;
            retVal[6] = (value & 64) > 0;
            retVal[7] = (value & 128) > 0;

            retVal[8] = (value & 256) > 0;
            retVal[9] = (value & 512) > 0;
            retVal[10] = (value & 1024) > 0;
            retVal[11] = (value & 2048) > 0;
            retVal[12] = (value & 4096) > 0;
            retVal[13] = (value & 8192) > 0;
            retVal[14] = (value & 16384) > 0;
            retVal[15] = (value & 32768) > 0;

            return retVal;
        }

        public static bool[] ToBits(this uint value)
        {
            var retVal = new bool[32];
            retVal[0] = (value & 1) > 0;
            retVal[1] = (value & 2) > 0;
            retVal[2] = (value & 4) > 0;
            retVal[3] = (value & 8) > 0;
            retVal[4] = (value & 16) > 0;
            retVal[5] = (value & 32) > 0;
            retVal[6] = (value & 64) > 0;
            retVal[7] = (value & 128) > 0;

            retVal[8] = (value & 256) > 0;
            retVal[9] = (value & 512) > 0;
            retVal[10] = (value & 1024) > 0;
            retVal[11] = (value & 2048) > 0;
            retVal[12] = (value & 4096) > 0;
            retVal[13] = (value & 8192) > 0;
            retVal[14] = (value & 16384) > 0;
            retVal[15] = (value & 32768) > 0;

            retVal[16] = (value & 65536) > 0;
            retVal[17] = (value & 131072) > 0;
            retVal[18] = (value & 262144) > 0;
            retVal[19] = (value & 524288) > 0;
            retVal[20] = (value & 1048576) > 0;
            retVal[21] = (value & 2097152) > 0;
            retVal[22] = (value & 4194304) > 0;
            retVal[23] = (value & 8388608) > 0;

            retVal[24] = (value & 16777216) > 0;
            retVal[25] = (value & 33554432) > 0;
            retVal[26] = (value & 67108864) > 0;
            retVal[27] = (value & 134217728) > 0;
            retVal[28] = (value & 268435456) > 0;
            retVal[29] = (value & 536870912) > 0;
            retVal[30] = (value & 1073741824) > 0;
            retVal[31] = (value & 2147483648) > 0;


            return retVal;
        }

        public static bool[] ToBits(this ulong value)
        {
            var retVal = new bool[64];
            retVal[0] = (value & 1) > 0;
            retVal[1] = (value & 2) > 0;
            retVal[2] = (value & 4) > 0;
            retVal[3] = (value & 8) > 0;
            retVal[4] = (value & 16) > 0;
            retVal[5] = (value & 32) > 0;
            retVal[6] = (value & 64) > 0;
            retVal[7] = (value & 128) > 0;

            retVal[8] = (value & 256) > 0;
            retVal[9] = (value & 512) > 0;
            retVal[10] = (value & 1024) > 0;
            retVal[11] = (value & 2048) > 0;
            retVal[12] = (value & 4096) > 0;
            retVal[13] = (value & 8192) > 0;
            retVal[14] = (value & 16384) > 0;
            retVal[15] = (value & 32768) > 0;

            retVal[16] = (value & 65536) > 0;
            retVal[17] = (value & 131072) > 0;
            retVal[18] = (value & 262144) > 0;
            retVal[19] = (value & 524288) > 0;
            retVal[20] = (value & 1048576) > 0;
            retVal[21] = (value & 2097152) > 0;
            retVal[22] = (value & 4194304) > 0;
            retVal[23] = (value & 8388608) > 0;

            retVal[24] = (value & 16777216) > 0;
            retVal[25] = (value & 33554432) > 0;
            retVal[26] = (value & 67108864) > 0;
            retVal[27] = (value & 134217728) > 0;
            retVal[28] = (value & 268435456) > 0;
            retVal[29] = (value & 536870912) > 0;
            retVal[30] = (value & 1073741824) > 0;
            retVal[31] = (value & 2147483648) > 0;

            retVal[32] = (value & 4294967296) > 0;
            retVal[33] = (value & 8589934592) > 0;
            retVal[34] = (value & 17179869184) > 0;
            retVal[35] = (value & 34359738368) > 0;
            retVal[36] = (value & 68719476736) > 0;
            retVal[37] = (value & 137438953472) > 0;
            retVal[38] = (value & 274877906944) > 0;
            retVal[39] = (value & 549755813888) > 0;

            retVal[40] = (value & 1099511627776) > 0;
            retVal[41] = (value & 2199023255552) > 0;
            retVal[42] = (value & 4398046511104) > 0;
            retVal[43] = (value & 8796093022208) > 0;
            retVal[44] = (value & 17592186044416) > 0;
            retVal[45] = (value & 35184372088832) > 0;
            retVal[46] = (value & 70368744177664) > 0;
            retVal[47] = (value & 140737488355328) > 0;

            retVal[48] = (value & 281474976710656) > 0;
            retVal[49] = (value & 562949953421312) > 0;
            retVal[50] = (value & 1125899906842624) > 0;
            retVal[51] = (value & 2251799813685248) > 0;
            retVal[52] = (value & 4503599627370496) > 0;
            retVal[53] = (value & 9007199254740992) > 0;
            retVal[54] = (value & 18014398509481984) > 0;
            retVal[55] = (value & 36028797018963968) > 0;

            retVal[56] = (value & 72057594037927936) > 0;
            retVal[57] = (value & 144115188075855872) > 0;
            retVal[58] = (value & 288230376151711744) > 0;
            retVal[59] = (value & 576460752303423488) > 0;
            retVal[60] = (value & 1152921504606846976) > 0;
            retVal[61] = (value & 2305843009213693952) > 0;
            retVal[62] = (value & 4611686018427387904) > 0;
            retVal[63] = (value & 9223372036854775808) > 0;

            return retVal;
        }


        public static ushort SortBitPair(this ushort value, int lowBit, int hiBit)
        {
            var hasLowBit = value.BitValue(lowBit);
            var hasHighBit = value.BitValue(hiBit);

            if (hasLowBit == hasHighBit) { return value; }
            if (hasHighBit) { return value; }

            return value.FilpBit(lowBit).FilpBit(hiBit);
        }

        public static byte ToByte(this bool[] bits)
        {
            byte bRet = 0;
            for (var i = bits.Length - 1; i > -1; i--)
            {
                bRet <<= 1;
                if (bits[i]) bRet += 1;
            }
            return bRet;
        }

        public static ushort ToUShort(this bool[] bits)
        {
            ushort bRet = 0;
            for (var i = bits.Length - 1; i > -1; i--)
            {
                bRet <<= 1;
                if (bits[i]) bRet += 1;
            }
            return bRet;
        }


        public static uint ToUInt(this bool[] bits)
        {
            uint bRet = 0;
            for (var i = bits.Length - 1; i > -1; i--)
            {
                bRet <<= 1;
                if (bits[i]) bRet += 1;
            }
            return bRet;
        }

        public static int ToHash(this IReadOnlyList<bool> bits)
        {
            if (bits.Count < 17)
            {
                return Hash0To16(bits);
            }
            return Hash0To16(bits) * Hash16To32(bits);
        }

        static int Hash16To32(IReadOnlyList<bool> bits)
        {
            var bRet = 0;
            for (var i = bits.Count - 1; i > 15; i--)
            {
                bRet <<= 1;
                if (bits[i]) bRet += 1;
            }
            return bRet;
        }

        static int Hash0To16(IReadOnlyList<bool> bits)
        {
            var bRet = 0;
            for (var i = bits.Count - 1; i > -1; i--)
            {
                bRet <<= 1;
                if (bits[i]) bRet += 1;
            }
            return bRet;
        }

        public static ulong ToULong(this bool[] bits)
        {
            ulong bRet = 0;
            for (var i = bits.Length - 1; i > -1; i--)
            {
                bRet <<= 1;
                if (bits[i]) bRet += 1;
            }
            return bRet;
        }

        public static byte ToByte(this IEnumerable<bool> bools)
        {
            return bools.ToArray().ToByte();
        }

        public static ushort ToUShort(this IEnumerable<bool> bools)
        {
            return bools.ToArray().ToUShort();
        }

        public static uint ToUInt(this IEnumerable<bool> bools)
        {
            return bools.ToArray().ToUInt();
        }

    }
}
