using System.Linq;
using KeypairSorting.Resources;
using KeypairSorting.ViewModels.Parts;
using SorterEvo.Workflows;
using WpfUtils;

namespace KeypairSorting.ViewModels.Scp
{
    public class ConfigScpVm : ViewModelBase, ICreateRunSelectorVm
    {
        public ConfigScpVm(IScpParams scpParams)
        {
            ConfigScpParamVm = new ConfigScpParamVm(scpParams);
            SorterGenomeEvalGridVm = new SorterGenomeEvalGridVm("Progenitors");
        }

        public CreateRunTemplateType CreateRunTemplateType
        {
            get { return CreateRunTemplateType.Create; }
        }

        public string Description
        {
            get { return "Config sorter tune"; }
        }

        public ConfigScpParamVm ConfigScpParamVm { get; set; }

        public SorterGenomeEvalGridVm SorterGenomeEvalGridVm { get; set; }

        public bool IsValid
        {
            get
            {
                return 
                    SorterGenomeEvalGridVm.SorterGenomeEvalVms.Any()
                    &&
                    ConfigScpParamVm.HasValidData;
            }
        }

    }
}
