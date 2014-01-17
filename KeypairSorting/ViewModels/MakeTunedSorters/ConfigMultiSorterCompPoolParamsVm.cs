using System.Windows.Input;
using KeypairSorting.Resources;
using WpfUtils;

namespace KeypairSorting.ViewModels.MakeTunedSorters
{
    public class ConfigMultiSorterCompPoolParamsVm : ViewModelBase, IConfigRunSelectorVm
    {
        public ConfigMultiSorterCompPoolParamsVm(ICommand runMultiTunedSortersCommand)
        {
            RunMultiTunedSortersCommand = runMultiTunedSortersCommand;
        }

        public ConfigRunTemplateType ConfigRunTemplateType
        {
            get { return ConfigRunTemplateType.Config; }
        }

        public string Description
        {
            get { return "Config sorter tune"; }
        }

        public ICommand RunMultiTunedSortersCommand { get; private set; }
    }
}
