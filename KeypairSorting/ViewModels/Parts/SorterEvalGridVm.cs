using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Input;
using WpfUtils;

namespace KeypairSorting.ViewModels.Parts
{
    public class SorterEvalGridVm : ViewModelBase
    {
        private ObservableCollection<ISorterEvalVm> _sorterOnSwitchableGroupVms
                = new ObservableCollection<ISorterEvalVm>();
        public ObservableCollection<ISorterEvalVm> SorterOnSwitchableGroupVms
        {
            get { return _sorterOnSwitchableGroupVms; }
            set { _sorterOnSwitchableGroupVms = value; }
        }

        #region CopyGridCommand

        RelayCommand _copyGridCommand;
        public ICommand CopyGridCommand
        {
            get
            {
                return _copyGridCommand ?? (_copyGridCommand
                    = new RelayCommand
                        (
                            param => OnCopyGridCommand(),
                            param => CanCopyGridCommand()
                        ));
            }
        }

        protected void OnCopyGridCommand()
        {
            var sb = new StringBuilder();
            sb.AppendLine(
                "SwitchesUsed" + "\t" +
                "Success" + "\t" +
                "SorterJson" + "\t" +
                "Switchables" + "\t" +
                "Sorter"
              );

            foreach (var sorterOnSwitchableGroupVm in SorterOnSwitchableGroupVms)
            {
                sb.AppendLine(
                                sorterOnSwitchableGroupVm.SwitchesUsed + "\t" +
                                sorterOnSwitchableGroupVm.Success + "\t" +
                                sorterOnSwitchableGroupVm.SorterJson + "\t" +
                                sorterOnSwitchableGroupVm.SwitchableGroupGuid + "\t" +
                                sorterOnSwitchableGroupVm.SorterGuid
                              );
            }

            Clipboard.SetText(sb.ToString());
        }

        bool CanCopyGridCommand()
        {
            return true;
        }

        #endregion // CopyGridCommand

        #region CopyResultsCommand

        RelayCommand _copyResultsCommand;
        public ICommand CopyResultsCommand
        {
            get
            {
                return _copyResultsCommand ?? (_copyResultsCommand
                    = new RelayCommand
                        (
                            param => OnCopyResultsCommand(),
                            param => CanCopyResultsCommand()
                        ));
            }
        }

        protected void OnCopyResultsCommand()
        {
            var sb = new StringBuilder();

            sb.AppendLine(
                "SwitchesUsed" + "\t" +
                "Success" + "\t" +
                "Switchables" + "\t" +
                "Sorter"
              );

            foreach (var sorterOnSwitchableGroupVm in SorterOnSwitchableGroupVms)
            {
                sb.AppendLine(
                                sorterOnSwitchableGroupVm.SwitchesUsed + "\t" +
                                sorterOnSwitchableGroupVm.Success + "\t" +
                                sorterOnSwitchableGroupVm.SwitchableGroupGuid + "\t" +
                                sorterOnSwitchableGroupVm.SorterGuid
                              );
            }

            Clipboard.SetText(sb.ToString());
        }

        bool CanCopyResultsCommand()
        {
            return true;
        }

        #endregion // CopyGridCommand

    }
}
