using System.Windows.Input;
using KeypairSorting.Resources;
using WpfUtils;

namespace KeypairSorting.ViewModels.MakeTunedSorters
{
    public class MultiConfigScpParamVm : ViewModelBase, IConfigRunSelectorVm
    {
        public MultiConfigScpParamVm(ICommand runMultiTunedSortersCommand)
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
