using System;
using System.Windows.Input;
using KeypairSorting.Resources;
using SorterEvo.TestData;
using WpfUtils;

namespace KeypairSorting.ViewModels.Scp
{
    public class ScpVm : ViewModelBase, IToolTemplateVm
    {
        public ScpVm()
        {
            _createScpParamVm = new CreateScpVm(
                    Layers.ScpParams(),
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
