using System;
using System.Linq;
using Genomic;
using Sorting.Switchables;

namespace SorterEvo
{
    public interface ISwitchableGroupGenome : ISimpleGenome<IChromosome>
    {
        int KeyCount { get; }
        SwitchableGroupGenomeType SwitchableGroupGenomeType { get; }
    }

    public static class SwitchableGroupGenome 
    {
        public static ISwitchableGroupGenome ToSwitchableGroupGenome(ISwitchableGroup switchableGroup)
        {
            if (switchableGroup.SwitchableDataType == typeof(uint))
            {
                var symbolCount = (uint)(2 ^ switchableGroup.KeyCount);
                return new SwitchableGroupGenomeImpl
                    (
                        guid: switchableGroup.Guid,
                        keyCount: switchableGroup.KeyCount,
                        chromosome: ((ISwitchableGroup<uint>)switchableGroup).Switchables
                                            .Select(t => t.Item)
                                            .ToUniformChromosome(Guid.NewGuid(), symbolCount),
                        switchableGroupGenomeType: SwitchableGroupGenomeType.UInt
                    );
            }

            if (switchableGroup.SwitchableDataType == typeof(ulong))
            {
                return new SwitchableGroupGenomeImpl
                    (
                        guid: switchableGroup.Guid,
                        keyCount: switchableGroup.KeyCount,
                        chromosome: null,
                        switchableGroupGenomeType: SwitchableGroupGenomeType.ULong
                    );
            }

            if (switchableGroup.SwitchableDataType == typeof(int[]))
            {
                var symbolCount = (uint)switchableGroup.KeyCount;
                return new SwitchableGroupGenomeImpl
                    (
                        guid: switchableGroup.Guid,
                        keyCount: switchableGroup.KeyCount,
                        chromosome: ((ISwitchableGroup<int[]>)switchableGroup).Switchables
                                            .SelectMany(t => t.Item.Cast<uint>())
                                            .ToUniformChromosome(Guid.NewGuid(), symbolCount),
                        switchableGroupGenomeType: SwitchableGroupGenomeType.IntArray
                    );
            }

            if (switchableGroup.SwitchableDataType == typeof(bool[]))
            {
                var symbolCount = (uint)switchableGroup.KeyCount;
                return new SwitchableGroupGenomeImpl
                    (
                        guid: switchableGroup.Guid,
                        keyCount: switchableGroup.KeyCount,
                        chromosome: null,
                        switchableGroupGenomeType: SwitchableGroupGenomeType.BitArray
                    );
            }

            throw new Exception(string.Format("data type {0} not  handled", switchableGroup.SwitchableDataType.Name));
        }
    }

    class SwitchableGroupGenomeImpl : SimpleGenomeImpl<IChromosome>, ISwitchableGroupGenome
    {
        public SwitchableGroupGenomeImpl
            (
                Guid guid,
                int keyCount,
                IChromosome chromosome,
                SwitchableGroupGenomeType switchableGroupGenomeType
            )
            : base
            (
                guid: guid,
                chromosome: chromosome,
                parentGuid: Guid.Empty
            )
        {
            _keyCount = keyCount;
            _switchableGroupGenomeType = switchableGroupGenomeType;
        }


        private readonly int _keyCount;
        public int KeyCount
        {
            get { return _keyCount; }
        }

        private readonly SwitchableGroupGenomeType _switchableGroupGenomeType;
        public SwitchableGroupGenomeType SwitchableGroupGenomeType
        {
            get { return _switchableGroupGenomeType; }
        }
    }
}
