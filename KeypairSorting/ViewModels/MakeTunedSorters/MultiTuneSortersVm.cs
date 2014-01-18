using System.Windows.Input;
using KeypairSorting.Resources;
using WpfUtils;

namespace KeypairSorting.ViewModels.MakeTunedSorters
{
    public class MultiTuneSortersVm : ViewModelBase, IToolTemplateVm
    {
        public MultiTuneSortersVm()
        {
            ConfigRunSelectorVm = new MultiConfigScpParamVm(RunMultiTuneSortersCommand);
        }

        public ToolTemplateType ToolTemplateType
        {
            get { return ToolTemplateType.MultiSorterTune; }
        }

        public string Description
        {
            get { return "Optimize Sorters Ensembles"; }
        }

        #region RunTunedSortersCommand

        RelayCommand _runMultiTuneSortersCommand;
        public ICommand RunMultiTuneSortersCommand
        {
            get
            {
                return _runMultiTuneSortersCommand ?? (_runMultiTuneSortersCommand
                    = new RelayCommand
                        (
                            param => OnRunMultiTuneSortersCommand(),
                            param => CanRunMultiTuneSortersCommand()
                        ));
            }
        }

        void OnRunMultiTuneSortersCommand()
        {
            ConfigRunSelectorVm = new RunMultiTuneSortersVm();
        }

        bool CanRunMultiTuneSortersCommand()
        {
            var configSorterPoolParams = _configRunSelectorVm as ConfigScpParamVm;

            return (configSorterPoolParams == null) || configSorterPoolParams.HasValidData;
        }

        #endregion // RunTunedSortersCommand

        private IConfigRunSelectorVm _configRunSelectorVm;
        public IConfigRunSelectorVm ConfigRunSelectorVm
        {
            get { return _configRunSelectorVm; }
            set
            {
                _configRunSelectorVm = value;
                OnPropertyChanged("ConfigRunSelectorVm");
            }
        }
    }
}
