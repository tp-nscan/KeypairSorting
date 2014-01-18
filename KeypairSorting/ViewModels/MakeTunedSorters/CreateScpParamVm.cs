using System;
using System.Text;
using System.Windows;
using System.Windows.Input;
using KeypairSorting.Resources;
using SorterEvo.Workflows;
using WpfUtils;

namespace KeypairSorting.ViewModels.MakeTunedSorters
{
    public class CreateScpParamVm : ViewModelBase, IConfigRunSelectorVm
    {

        public CreateScpParamVm(IScpParams scpParams, ICommand runTunedSortersCommand)
        {
            RunTunedSortersCommand = runTunedSortersCommand;
            ConfigScpParamVm = new ConfigScpParamVm(scpParams);
        }

        public ICommand RunTunedSortersCommand { get; private set; }

        public ConfigRunTemplateType ConfigRunTemplateType
        {
            get { return ConfigRunTemplateType.Config; }
        }

        public string Description
        {
            get { return "Config sorter tune"; }
        }

        public ConfigScpParamVm ConfigScpParamVm { get; set; }
    }


}
