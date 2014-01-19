using System;
using System.Linq;
using System.Windows.Input;
using KeypairSorting.Resources;
using SorterEvo.Workflows;
using WpfUtils;

namespace KeypairSorting.ViewModels.MakeTunedSorters
{
    public class ScpVm : ViewModelBase, IToolTemplateVm
    {
        public ScpVm()
        {
            _createScpParamVm = new CreateScpVm(
                    ScpParams.Make(
                        sorterLayerStartingGenomeCount: 10,
                        sorterLayerExpandedGenomeCount: 30,
                        sorterMutationRate: 0.03,
                        sorterInsertionRate: 0.03,
                        sorterDeletionRate: 0.03,
                        name: "standard",
                        seed: 1234,
                        totalGenerations: 10
                    ),
                    CreateMiltiScpCommand
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

        #region CreateScpCommand

        RelayCommand _createScpCommand;
        public ICommand CreateMiltiScpCommand
        {
            get
            {
                return _createScpCommand ?? (_createScpCommand
                    = new RelayCommand
                        (
                            param => OnCreateMultiScpCommand(),
                            param => CanCreateMultiScpCommand()
                        ));
            }
        }

        void OnCreateMultiScpCommand()
        {
            var createScpVm = ConfigRunSelectorVm as CreateScpVm;
            if (createScpVm == null)
            {
                throw new Exception("CreateScpVm is null");
            }

            ConfigRunSelectorVm = new RunScpVm
                (
                    scpParams: createScpVm.ConfigScpVm.ConfigScpParamVm.GetParams,
                    sorterGenomeEvalVms: createScpVm.ConfigScpVm.SorterGenomeEvalGridVm.SorterGenomeEvalVms
                );
        }

        bool CanCreateMultiScpCommand()
        {
            var createScpVm = _createScpParamVm as CreateScpVm;

            return  createScpVm != null
                    &&
                    createScpVm.ConfigScpVm.IsValid;
        }

        #endregion // CreateScpCommand

        private IConfigRunSelectorVm _createScpParamVm;
        public IConfigRunSelectorVm ConfigRunSelectorVm
        {
            get { return _createScpParamVm; }
            set
            {
                _createScpParamVm = value;
                OnPropertyChanged("ConfigRunSelectorVm");
            }
        }
    }
}
