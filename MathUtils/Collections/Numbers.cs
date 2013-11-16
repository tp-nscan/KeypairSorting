namespace MathUtils.Collections
{
    public static class Numbers
    {
        static Numbers()
        {
            UintMasks = new uint[32];
            UintMasks[0] = 1;
            for (var i = 1; i < 32; i++)
            {
                UintMasks[i] = (UintMasks[i - 1] << 1) + 1;
            }

            UlongMasks = new ulong[64];
            UlongMasks[0] = 1;
            for (var i = 1; i < 64; i++)
            {
                UlongMasks[i] = (UlongMasks[i - 1] << 1) + 1;
            }
        }

        private static readonly uint[] UintMasks;
        private static readonly ulong[] UlongMasks;

        public static uint UintMask(int bitCount)
        {
            return UintMasks[bitCount];
        }

        public static ulong UlongMask(int bitCount)
        {
            return UlongMasks[bitCount];
        }

        public static int HiBit(this ulong value)
        {
            var curVal = value;
            for (var i = -1; i < 64; i++)
            {
                if (curVal == 0)
                {
                    return i;
                }
                curVal >>= 1;
            }
            return -1;
        }

        public static int HiBit(this uint value)
        {
            var curVal = value;
            for (var i = -1; i < 32; i++)
            {
                if (curVal == 0)
                {
                    return i;
                }
                curVal >>= 1;
            }
            return -1;
        }

    }
}
