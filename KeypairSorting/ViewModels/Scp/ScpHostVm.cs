using System;
using System.Windows.Input;
using KeypairSorting.Resources;
using SorterEvo.TestData;
using WpfUtils;

namespace KeypairSorting.ViewModels.Scp
{
    public class ScpHostVm : ViewModelBase, IToolTemplateVm
    {
        public ScpHostVm()
        {
            _createRunSelectorVm = new CreateScpVm(
                    Layers.ScpParams(),
                    CreateRunnerCommand
                );
        }

        public ToolTemplateType ToolTemplateType
        {
            get { return ToolTemplateType.SorterTune; }
        }

        public string Description
        {
            get { return "Optimize Sorters"; }
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
            var createScpVm = CreateRunSelectorVm as CreateScpVm;
            if (createScpVm == null)
            {
                throw new Exception("CreateScpVm is null");
            }

            CreateRunSelectorVm = new RunScpVm
                (
                    scpParams: createScpVm.ConfigScpVm.ConfigScpParamVm.GetParams,
                    sorterGenomeEvalVms: createScpVm.ConfigScpVm.SorterGenomeEvalGridVm.SorterGenomeEvalVms
                );
        }

        bool CanCreateRunnerCommand()
        {
            var createScpVm = _createRunSelectorVm as CreateScpVm;

            return  createScpVm != null
                    &&
                    createScpVm.ConfigScpVm.IsValid;
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
