using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Sorting.CompetePools;
using Sorting.Json.Sorters;
using Sorting.Sorters;
using WpfUtils;

namespace KeypairSorting.ViewModels.Parts
{
    public interface ISorterEvalVm
    {
        string SorterJson { get; }
        bool Success { get; }
        int UseCount { get; }
        Guid SwitchableGuid { get; }
        string SwitchUseList { get; }
        ISorterEval GetSorterEval();
    }

    public static class SorterEvalVm
    {
        public static ISorterEvalVm ToSorterEvalVm(this ISorterEval sorterEval)
        {
            return new SorterEvalVmImpl(sorterEval);
        }

        public static ISorterEvalVm ToSorterEvalVm(this string reportString)
        {
            var pcs = reportString.Split("\t".ToCharArray());
            ISorter sorter;
            var switchableGuid = Guid.Empty;
            var success = false;
            var useCount = 0;

            if (pcs.Count() > 2)
            {
                sorter = pcs[0].ToSorter();
                bool.TryParse(pcs[1], out success);
                int.TryParse(pcs[2], out useCount);
            }
            else
            {
                return new SorterEvalVmImpl(null);
            }

            if (pcs.Count() > 3)
            {

                Guid.TryParse(pcs[3], out switchableGuid);
            }
            else
            {
                return new SorterEvalVmImpl
                (
                    SorterEval.Make
                    (
                        sorter: sorter,
                        switchableGroupGuid: switchableGuid,
                        success: success,
                        switchUseCount: useCount
                    )
                );
            }

            var useList = JsonConvert.DeserializeObject<List<double>>(pcs[4]);

            return new SorterEvalVmImpl
            (
                SorterEval.Make
                (
                    sorter: sorter,
                    switchableGroupGuid: switchableGuid,
                    success: success,
                    switchUseList: useList
                )
            );
        }
    }

    class SorterEvalVmImpl : ViewModelBase, ISorterEvalVm
    {
        public SorterEvalVmImpl(ISorterEval sorterEval)
        {
            _sorterEval = sorterEval;
        }

        private readonly ISorterEval _sorterEval;
        public ISorterEval GetSorterEval()
        {
            return _sorterEval;
        }

        public int UseCount { get { return GetSorterEval().SwitchUseCount; } }

        public string SorterJson
        {
            get
            {
                return (GetSorterEval().Sorter == null) ?
                    string.Empty :
                    GetSorterEval().Sorter.ToJsonString();
            }
        }

        public Guid SorterGuid
        {
            get { return GetSorterEval().Sorter.Guid; }
        }

        public bool Success { get { return GetSorterEval().Success; } }

        public Guid SwitchableGuid { get { return GetSorterEval().SwitchableGroupGuid; } }

        public string SwitchUseList
        {
            get
            {
                return JsonConvert.SerializeObject
                    (
                        GetSorterEval().SwitchUseList, 
                        Formatting.None
                    );
            }
        }

    }
}
