using KeypairSorting.Resources;
using WpfUtils;

namespace KeypairSorting.ViewModels.MakeTunedSorters
{
    public class RunMultiTuneSortersVm : ViewModelBase, IConfigRunSelectorVm
    {

        public ConfigRunTemplateType ConfigRunTemplateType
        {
            get { return ConfigRunTemplateType.Run; }
        }

        public string Description
        {
            get { return "Tune sorters"; }
        }
    }
}
