using KeypairSorting.Resources;
using KeypairSorting.Views;
using WpfUtils;

namespace KeypairSorting.ViewModels
{
    public class MakeSorterTuneVm : ViewModelBase, IToolTemplateVm
    {
        public MakeSorterTuneVm()
        {
            
        }

        public ToolTemplateType ToolTemplateType
        {
            get { return ToolTemplateType.SorterTune; }
        }


        public string Description
        {
            get { return "Optimize Sorters"; }
        }
    }
}
