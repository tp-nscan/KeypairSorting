using System;
using System.Collections.Generic;
using System.Linq;
using MathUtils.Collections;
using Sorting.KeyPairs;
using Sorting.Sorters;
using Sorting.Switchables;

namespace Sorting.CompetePools
{
    public interface ISorterEval
    {
        ISorter Sorter { get; }
        IReadOnlyList<double> SwitchUseList { get; }
        ISwitchableGroup CoveringSet { get; } 
        int SwitchUseCount { get; }
        Guid SwitchableGroupGuid { get; }
        bool Success { get; }
    }

    public static class SorterEval
    {
        public static ISorterEval Make
        (
            ISorter sorter,
            Guid switchableGroupGuid,
            bool success,
            int switchUseCount,
            ISwitchableGroup coveringSet
        )
        {
            return new SorterEvalImpl
                (
                    sorter: sorter,
                    switchableGroupGuid: switchableGroupGuid,
                    switchUseList: null,
                    success: success,
                    switchUseCount: switchUseCount,
                    coveringSet: coveringSet
                );
        }

        public static ISorterEval Make
            (
                ISorter sorter,
                Guid switchableGroupGuid,
                bool success,
                IReadOnlyList<double> switchUseList,
                ISwitchableGroup coveringSet
            )
        {
            return new SorterEvalImpl
                (
                    sorter: sorter,
                    switchableGroupGuid: switchableGroupGuid,
                    switchUseList: switchUseList, 
                    success: success,
                    switchUseCount: (switchUseList == null) ? 0 : switchUseList.Count(t => t > 0),
                    coveringSet: coveringSet
                );    
        }

        public static IEnumerable<IKeyPair> UsedKeyPairs(this ISorterEval sorterEval)
        {
            for (var dex = 0; dex < sorterEval.Sorter.KeyPairCount; dex++)
            {
                if (sorterEval.SwitchUseList[dex] > 0)
                {
                   yield return sorterEval.Sorter.KeyPair(dex);
                }
            }
        }

        public static ISorter Reduce(this ISorterEval sorterEval, Guid guid)
        {
            return sorterEval.UsedKeyPairs().ToSorter(sorterEval.Sorter.Guid, sorterEval.Sorter.KeyCount);
        }

        public static ulong Hash(this IReadOnlyList<int> intList, int start)
        {
            ulong i = 57;
            return intList
                        .Repeat()
                        .Skip(start)
                        .Take(30)
                        .Aggregate((ulong)377, (o, n) => (ulong)(n + 1) * (o + i++));
        }

        public static ulong Hash(this IEnumerable<int> intList)
        {
            ulong i = 57;
            return intList
                        .Aggregate((ulong)377, (o, n) => (ulong)(n + 1) * (o + i++));
        }
    }

    public class SorterEvalImpl : ISorterEval
    {
        private readonly ISorter _sorter;

        public SorterEvalImpl
        (
            ISorter sorter,
            Guid switchableGroupGuid,
            IReadOnlyList<double> switchUseList, 
            bool success,
            int switchUseCount,
            ISwitchableGroup coveringSet
        )
        {
            _sorter = sorter;
            _switchUseList = switchUseList;
            _success = success;
            _switchableGroupGuid = switchableGroupGuid;
            _switchUseCount = switchUseCount;
            _coveringSet = coveringSet;
        }

        private readonly Guid _switchableGroupGuid;
        public Guid SwitchableGroupGuid
        {
            get { return _switchableGroupGuid; }
        }

        public ISorter Sorter
        {
            get { return _sorter; }
        }

        private readonly bool _success;
        public bool Success
        {
            get { return _success; }
        }

        private readonly IReadOnlyList<double> _switchUseList;
        public IReadOnlyList<double> SwitchUseList
        {
            get { return _switchUseList; }
        }

        private readonly ISwitchableGroup _coveringSet;
        public ISwitchableGroup CoveringSet
        {
            get { return _coveringSet; }
        }

        private readonly int _switchUseCount;
        public int SwitchUseCount
        {
            get { return _switchUseCount; }
        }
    }
}
