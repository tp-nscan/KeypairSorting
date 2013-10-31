namespace Sorting.SwitchFunctionSets
{
    public class SortedNumberEval
    {
        static SortedNumberEval()
        {
            IsSortedMap[0] = true;
            IsSortedMap[32768] = true;
            IsSortedMap[49152] = true;
            IsSortedMap[57344] = true;
            IsSortedMap[61440] = true;
            IsSortedMap[63488] = true;
            IsSortedMap[64512] = true;
            IsSortedMap[65024] = true;
            IsSortedMap[65280] = true;
            IsSortedMap[65408] = true;
            IsSortedMap[65472] = true;
            IsSortedMap[65504] = true;
            IsSortedMap[65520] = true;
            IsSortedMap[65528] = true;
            IsSortedMap[65532] = true;
            IsSortedMap[65534] = true;
            IsSortedMap[65535] = true;
        }

        static readonly bool[] IsSortedMap = new bool[65536];

        public static bool IsSorted(ushort switchable)
        {
            return IsSortedMap[switchable];
        }

        public static bool IsSortedForSmall(uint switchable)
        {
            return IsSortedMap[switchable];
        }

        public static bool IsSorted(uint switchable)
        {
            var lowPart = (ushort) switchable;
            var hiPart = (ushort) (switchable >> 16);

            if (hiPart == 65535)
            {
                return IsSortedMap[lowPart];
            }

            return lowPart == 0 && IsSortedMap[hiPart];
        }

        public static bool IsSorted(ulong switchable)
        {
            var part0 = (ushort)switchable;
            var part1 = (ushort)(switchable >> 16);
            var part2 = (ushort)(switchable >> 32);
            var part3 = (ushort)(switchable >> 48);

            if (part3 == 65535)
            {
                if (part2 == 65535)
                {
                    if (part1 == 65535)
                    {
                        return IsSortedMap[part0];
                    }
                    return part0 == 0 && IsSortedMap[part1];
                }
                return part0 == 0 && part1 == 0 &&  IsSortedMap[part2];
            }
            return part0 == 0 && part1 == 0 && part2 == 0 && IsSortedMap[part3];
        }
    }
}
