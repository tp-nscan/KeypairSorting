using System;
using System.Windows.Input;
using KeypairSorting.Resources;
using KeypairSorting.ViewModels.Scp;
using WpfUtils;

namespace KeypairSorting.ViewModels.MutateSorters
{
    public class MutateSortersHostVm : ViewModelBase, IToolTemplateVm
    {
        public MutateSortersHostVm()
        {
            _createRunSelectorVm = new CreateMutateSortersVm(SorterEvo.TestData.Layers.SorterMutateParams(), CreateRunnerCommand);
            //_sorterGenomeEvalGridVmInitial = new SorterGenomeEvalGridVm("Parents");
            //_sorterGenomeEvalGridVm = new SorterGenomeEvalGridVm("Mutants");
        }

        public ToolTemplateType ToolTemplateType
        {
            get { return ToolTemplateType.SorterMutate; }
        }

        public string Description
        {
            get { return "Convert Sorters to SorterGenomes"; }
        }

        #region CreateRunnerCommand

        RelayCommand _createRunnerCommand;
        public ICommand CreateRunnerCommand
        {
            get
            {
                return _createRunnerCommand ?? (_createRunnerCommand
                    = new RelayCommand
                        (
                            param => OnCreateRunnerCommand(),
                            param => CanCreateRunnerCommand()
                        ));
            }
        }

        void OnCreateRunnerCommand()
        {
            var createScpVm = CreateRunSelectorVm as CreateMutateSortersVm;
            if (createScpVm == null)
            {
                throw new Exception("CreateScpVm is null");
            }

            CreateRunSelectorVm = new RunMutateSortersVm
                (
                    sorterMutateParams: createScpVm.ConfigMutateSortersVm.ConfigMutateSortersParamVm.GetParams,
                    sorterGenomeEvalVms: createScpVm.ConfigMutateSortersVm.SorterGenomeEvalGridVm.SorterGenomeEvalVms
                );
        }

        bool CanCreateRunnerCommand()
        {
            var configRunSelector = _createRunSelectorVm as CreateMutateSortersVm;

            return configRunSelector != null
                    &&
                    configRunSelector.ConfigMutateSortersVm.IsValid;
        }

        #endregion // CreateRunnerCommand

        private ICreateRunSelectorVm _createRunSelectorVm;
        public ICreateRunSelectorVm CreateRunSelectorVm
        {
            get { return _createRunSelectorVm; }
            set
            {
                _createRunSelectorVm = value;
                OnPropertyChanged("CreateRunSelectorVm");
            }
        }

    }
}
