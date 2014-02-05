using System.Linq;
using KeypairSorting.Resources;
using KeypairSorting.ViewModels.Parts;
using SorterEvo.Workflows;
using WpfUtils;

namespace KeypairSorting.ViewModels.MutateSorters
{
    public class ConfigMutateSortersVm : ViewModelBase
    {
        public ConfigMutateSortersVm(ISorterMutateParams sorterMutateParams)
        {
            ConfigMutateSortersParamVm = new ConfigMutateSortersParamVm(sorterMutateParams);
            SorterGenomeEvalGridVm = new SorterGenomeEvalGridVm("Progenitors");
        }

        public SorterGenomeEvalGridVm SorterGenomeEvalGridVm { get; set; }

        public ConfigMutateSortersParamVm ConfigMutateSortersParamVm { get; set; }

        public bool IsValid
        {
            get
            {
                return
                    SorterGenomeEvalGridVm.SorterGenomeEvalVms.Any()
                    &&
                    ConfigMutateSortersParamVm.HasValidData;
            }
        }
    }
}
