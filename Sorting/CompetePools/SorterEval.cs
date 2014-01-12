﻿using System;
using System.Collections.Generic;
using System.Linq;
using Sorting.Sorters;

namespace Sorting.CompetePools
{
    public interface ISorterEval
    {
        ISorter Sorter { get; }
        IReadOnlyList<double> SwitchUseList { get; }
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
            int switchUseCount
        )
        {
            return new SorterEvalImpl
                (
                    sorter: sorter,
                    switchableGroupGuid: switchableGroupGuid,
                    switchUseList: null,
                    success: success,
                    switchUseCount: switchUseCount
                );
        }

        public static ISorterEval Make
            (
                ISorter sorter,
                Guid switchableGroupGuid,
                bool success,
                IReadOnlyList<double> switchUseList
            )
        {
            return new SorterEvalImpl
                (
                    sorter: sorter,
                    switchableGroupGuid: switchableGroupGuid,
                    switchUseList: switchUseList, 
                    success: success,
                    switchUseCount: (switchUseList == null) ? 0 : switchUseList.Count(t => t > 0)
                );    
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
            int switchUseCount
        )
        {
            _sorter = sorter;
            _switchUseList = switchUseList;
            _success = success;
            _switchableGroupGuid = switchableGroupGuid;
            _switchUseCount = switchUseCount;
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

        private readonly int _switchUseCount;
        public int SwitchUseCount
        {
            get { return _switchUseCount; }
        }
    }
}
