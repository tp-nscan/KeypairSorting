using System.Linq;
using KeypairSorting.Resources;
using KeypairSorting.ViewModels.Parts;
using SorterEvo.Workflows;
using WpfUtils;

namespace KeypairSorting.ViewModels.Scp
{
    public class ConfigScpVm : ViewModelBase, IConfigRunSelectorVm
    {
        public ConfigScpVm(IScpParams scpParams)
        {
            ConfigScpParamVm = new ConfigScpParamVm(scpParams);
            SorterGenomeEvalGridVm = new SorterGenomeEvalGridVm("Progenitors");
        }

        public ConfigRunTemplateType ConfigRunTemplateType
        {
            get { return ConfigRunTemplateType.Config; }
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
