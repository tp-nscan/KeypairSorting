using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using KeypairSorting.Resources;
using SorterEvo.TestData;
using SorterEvo.Workflows;
using WpfUtils;

namespace KeypairSorting.ViewModels.MakeTunedSorters
{
    public class MultiCreateScpVm : ViewModelBase, IConfigRunSelectorVm
    {
        public MultiCreateScpVm(ICommand createMultiScpCommand)
        {
            CreateMultiScpCommand = createMultiScpCommand;
        }

        public ConfigRunTemplateType ConfigRunTemplateType
        {
            get { return ConfigRunTemplateType.Config; }
        }

        public string Description
        {
            get { return "Config sorter tune"; }
        }

        public ICommand CreateMultiScpCommand { get; private set; }

        private ObservableCollection<ConfigScpVm> _configScpVms
                = new ObservableCollection<ConfigScpVm>();
        public ObservableCollection<ConfigScpVm> ConfigScpVms
        {
            get { return _configScpVms; }
            set { _configScpVms = value; }
        }

        #region AddScpParamCommand

        RelayCommand _addScpParamCommand;
        public ICommand AddScpConfigCommand
        {
            get
            {
                return _addScpParamCommand ?? (_addScpParamCommand
                    = new RelayCommand
                        (
                            param => OnAddScpConfigCommand(),
                            param => CanAddScpConfigCommand()
                        ));
            }
        }

        protected void OnAddScpConfigCommand()
        {
            ConfigScpVms.Add(
                new ConfigScpVm(
                    scpParams: Layers.ScpParams()
                ));
        }

        bool CanAddScpConfigCommand()
        {
            return true;
        }

        #endregion // AddScpParamCommand


        #region RemoveScpConfigCommand

        RelayCommand _removeScpConfigCommand;
        public ICommand RemoveScpConfigCommand
        {
            get
            {
                return _removeScpConfigCommand ?? (_removeScpConfigCommand
                    = new RelayCommand
                        (
                            OnRemoveScpConfigCommand,
                            param => CanRemoveScpConfigCommand()
                        ));
            }
        }

        protected void OnRemoveScpConfigCommand(object param)
        {
            ConfigScpVms.Remove((ConfigScpVm)param);
        }

        bool CanRemoveScpConfigCommand()
        {
            return ConfigScpVms.Any();
        }

        #endregion // RemoveScpParamCommand

    }
}
