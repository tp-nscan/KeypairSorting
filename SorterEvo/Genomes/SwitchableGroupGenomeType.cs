using System;

namespace SorterEvo.Genomes
{
    public enum SwitchableGroupGenomeType
    {
        UInt,
        ULong,
        BitArray,
        IntArray
    }

    public static class SwitchableGroupGenomeTypeFuncs
    {
        public static Type ToSwitchableGroupDataType
        (
            this SwitchableGroupGenomeType switchableGroupGenomeType
        )
        {
            switch (switchableGroupGenomeType)
            {
                case SwitchableGroupGenomeType.UInt:
                    return typeof(uint);
                case SwitchableGroupGenomeType.ULong:
                    return typeof(ulong);
                case SwitchableGroupGenomeType.BitArray:
                    return typeof(bool[]);
                case SwitchableGroupGenomeType.IntArray:
                    return typeof(uint[]);
                default:
                    throw new Exception(string.Format("SwitchableGroupGenomeType: {0} unkown", switchableGroupGenomeType));
            } 

        }

    }
}