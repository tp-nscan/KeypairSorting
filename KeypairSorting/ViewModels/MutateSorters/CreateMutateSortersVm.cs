using KeypairSorting.Resources;
using WpfUtils;

namespace KeypairSorting.ViewModels.MutateSorters
{
    class CreateMutateSortersVm : ViewModelBase, IConfigRunSelectorVm
    {

        public ConfigRunTemplateType ConfigRunTemplateType
        {
            get { return ConfigRunTemplateType.Run; }
        }

        public string Description
        {
            get { return "Create mutated sorters"; }
        }
    }
}
