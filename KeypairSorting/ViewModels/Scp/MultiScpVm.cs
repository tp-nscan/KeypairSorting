using System.Linq;
using System.Windows.Input;
using KeypairSorting.Resources;
using WpfUtils;

namespace KeypairSorting.ViewModels.Scp
{
    public class MultiScpVm : ViewModelBase, IToolTemplateVm
    {
        public MultiScpVm()
        {
            ConfigRunSelectorVm = new MultiCreateScpVm(MultiCreateScpCommand);
        }

        public ToolTemplateType ToolTemplateType
        {
            get { return ToolTemplateType.MultiSorterTune; }
        }

        public string Description
        {
            get { return "Optimize Sorters Ensembles"; }
        }

        #region MultiCreateScpCommand

        RelayCommand _multiCreateScpCommand;
        public ICommand MultiCreateScpCommand
        {
            get
            {
                return _multiCreateScpCommand ?? (_multiCreateScpCommand
                    = new RelayCommand
                        (
                            param => OnMultiCreateScpCommand(),
                            param => CanMultiCreateScpCommand()
                        ));
            }
        }

        void OnMultiCreateScpCommand()
        {
            var multiCreateScpVm = _configRunSelectorVm as MultiCreateScpVm;

            // ReSharper disable PossibleNullReferenceException
               ConfigRunSelectorVm = new MultiRunScpVm(multiCreateScpVm.ConfigScpVms);
            // ReSharper restore PossibleNullReferenceException
        }

        bool CanMultiCreateScpCommand()
        {
            var multiCreateScpVm = _configRunSelectorVm as MultiCreateScpVm;

            return multiCreateScpVm != null
                   &&
                   multiCreateScpVm.ConfigScpVms.All(v => v.IsValid);
        }

        #endregion // MultiCreateScpCommand

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
