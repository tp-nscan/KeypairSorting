using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Input;
using WpfUtils;

namespace KeypairSorting.ViewModels
{
    public class SorterEvalGridVm : ViewModelBase
    {
        private ObservableCollection<ISorterOnSwitchableGroupVm> _sorterOnSwitchableGroupVms
                = new ObservableCollection<ISorterOnSwitchableGroupVm>();
        public ObservableCollection<ISorterOnSwitchableGroupVm> SorterOnSwitchableGroupVms
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

            foreach (var sorterOnSwitchableGroupVm in SorterOnSwitchableGroupVms)
            {
                sb.AppendLine(
                                sorterOnSwitchableGroupVm.SwitchesUsed + "\t" +
                                sorterOnSwitchableGroupVm.Success + "\t" +
                                sorterOnSwitchableGroupVm.SorterJson
                              );
            }

            Clipboard.SetText(sb.ToString());
        }

        bool CanCopyGridCommand()
        {
            return true;
        }

        #endregion // CopyGridCommand

    }
}
