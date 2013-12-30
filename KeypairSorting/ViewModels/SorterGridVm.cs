using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfUtils;

namespace KeypairSorting.ViewModels
{
    public class SorterGridVm : ViewModelBase
    {


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

            //foreach (var sorterOnSwitchableGroupVm in SorterOnSwitchableGroupVms)
            //{
            //    sb.AppendLine(
            //                    sorterOnSwitchableGroupVm.SwitchesUsed + "\t" +
            //                    sorterOnSwitchableGroupVm.Success + "\t" +
            //                    sorterOnSwitchableGroupVm.SorterJson
            //                  );
            //}

            Clipboard.SetText(sb.ToString());
        }

        bool CanCopyGridCommand()
        {
            return true;
        }

        #endregion // CopyGridCommand


        #region PasteGridCommand

        RelayCommand _pasteGridCommand;
        public ICommand PasteGridCommand
        {
            get
            {
                return _pasteGridCommand ?? (_pasteGridCommand
                    = new RelayCommand
                        (
                            param => OnPasteGridCommand(),
                            param => CanPasteGridCommand()
                        ));
            }
        }

        protected void OnPasteGridCommand()
        {
            var sb = new StringBuilder();

            //foreach (var sorterOnSwitchableGroupVm in SorterVms)
            //{
            //    sb.AppendLine(
            //                    sorterOnSwitchableGroupVm.SwitchesUsed + "\t" +
            //                    sorterOnSwitchableGroupVm.Success + "\t" +
            //                    sorterOnSwitchableGroupVm.SorterJson
            //                  );
            //}

            Clipboard.SetText(sb.ToString());
        }

        bool CanPasteGridCommand()
        {
            return Clipboard.ContainsText();
        }

        #endregion // PasteGridCommand


    }
}
