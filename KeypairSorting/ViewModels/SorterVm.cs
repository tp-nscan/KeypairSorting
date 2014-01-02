using Sorting.Json.Sorters;
using Sorting.Sorters;
using WpfUtils;

namespace KeypairSorting.ViewModels
{
    public class SorterVm : ViewModelBase    
    {
        public SorterVm(ISorter sorter)
        {
            _sorter = sorter;
        }

        private readonly ISorter _sorter;
        public string SorterJson
        {
            get
            {
                return _sorter.ToJsonString();
            }
        }
    }
}
