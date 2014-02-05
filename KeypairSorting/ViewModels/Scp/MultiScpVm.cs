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
            CreateRunSelectorVm = new MultiCreateScpVm(MultiCreateScpCommand);
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
            var multiCreateScpVm = _createRunSelectorVm as MultiCreateScpVm;

            // ReSharper disable PossibleNullReferenceException
               CreateRunSelectorVm = new MultiRunScpVm(multiCreateScpVm.ConfigScpVms);
            // ReSharper restore PossibleNullReferenceException
        }

        bool CanMultiCreateScpCommand()
        {
            var multiCreateScpVm = _createRunSelectorVm as MultiCreateScpVm;

            return multiCreateScpVm != null
                   &&
                   multiCreateScpVm.ConfigScpVms.All(v => v.IsValid);
        }

        #endregion // MultiCreateScpCommand

        private ICreateRunSelectorVm _createRunSelectorVm;
        public ICreateRunSelectorVm CreateRunSelectorVm
        {
            get { return _createRunSelectorVm; }
            set
            {
                _createRunSelectorVm = value;
                OnPropertyChanged("ConfigRunSelectorVm");
            }
        }
    }
}
