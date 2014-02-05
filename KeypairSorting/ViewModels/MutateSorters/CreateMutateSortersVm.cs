using System.Windows.Input;
using KeypairSorting.Resources;
using SorterEvo.Workflows;
using WpfUtils;

namespace KeypairSorting.ViewModels.MutateSorters
{
    class CreateMutateSortersVm : ViewModelBase, ICreateRunSelectorVm
    {
        public CreateMutateSortersVm(ISorterMutateParams mutateParams, ICommand createSorterMutateCommand)
        {
            CreateSorterMutateCommand = createSorterMutateCommand;
            ConfigMutateSortersVm = new ConfigMutateSortersVm(mutateParams);
        }

        public ICommand CreateSorterMutateCommand { get; private set; }

        public CreateRunTemplateType CreateRunTemplateType
        {
            get { return CreateRunTemplateType.Create; }
        }

        public string Description
        {
            get { return "Create mutated sorters"; }
        }


        public ConfigMutateSortersVm ConfigMutateSortersVm { get; set; }
    }
}
