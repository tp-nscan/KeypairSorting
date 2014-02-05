using System.Windows.Input;
using KeypairSorting.Resources;
using SorterEvo.Workflows;
using WpfUtils;

namespace KeypairSorting.ViewModels.Scp
{
    public class CreateScpVm : ViewModelBase, ICreateRunSelectorVm
    {
        public CreateScpVm(IScpParams scpParams, ICommand createScpCommand)
        {
            CreateScpCommand = createScpCommand;
            ConfigScpVm = new ConfigScpVm(scpParams);
        }

        public ICommand CreateScpCommand { get; private set; }

        public CreateRunTemplateType CreateRunTemplateType
        {
            get { return CreateRunTemplateType.Create; }
        }

        public string Description
        {
            get { return "Config sorter tune"; }
        }

        public ConfigScpVm ConfigScpVm { get; set; }

    }

}
