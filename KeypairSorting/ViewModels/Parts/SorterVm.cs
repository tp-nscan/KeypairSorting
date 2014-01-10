using System;
using Sorting.Json.Sorters;
using Sorting.Sorters;
using WpfUtils;

namespace KeypairSorting.ViewModels.Parts
{
    public class SorterVm : ViewModelBase    
    {
        public SorterVm(ISorter sorter)
        {
            _sorter = sorter;
        }

        private readonly ISorter _sorter;

        public Guid Guid
        {
            get { return _sorter.Guid; }
        }

        public string SorterJson
        {
            get { return _sorter.ToJsonString(); }
        }

        public ISorter Sorter
        {
            get { return _sorter; }
        }
    }
}
