using System;
using KeypairSorting.Resources;
using Sorting.Sorters;
using WpfUtils;

namespace KeypairSorting.ViewModels
{
    public class MakeSorterGenomesVm : ViewModelBase, IToolTemplateVm
    {

        public MakeSorterGenomesVm()
        {
            
        }

        public ToolTemplateType ToolTemplateType
        {
            get { return ToolTemplateType.SorterGenomeGen; }
        }

        public string Description
        {
            get { return "Convert Sorters to SorterGenomes"; }
        }
    }
}
