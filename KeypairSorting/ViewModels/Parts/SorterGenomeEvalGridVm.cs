using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using WpfUtils;

namespace KeypairSorting.ViewModels.Parts
{
    public class SorterGenomeEvalGridVm : ViewModelBase
    {
        private readonly ObservableCollection<ISorterGenomeEvalVm> _sorterGenomeVms = new ObservableCollection<ISorterGenomeEvalVm>();
        public ObservableCollection<ISorterGenomeEvalVm> SorterGenomeEvalVms
        {
            get { return _sorterGenomeVms; }
        }

        #region ClearGridCommand

        RelayCommand _clearGridCommand;
        public ICommand ClearGridCommand
        {
            get
            {
                return _clearGridCommand ?? (_clearGridCommand
                    = new RelayCommand
                        (
                            param => OnClearGridCommand(),
                            param => CanClearGridCommand()
                        ));
            }
        }

        protected void OnClearGridCommand()
        {
            SorterGenomeEvalVms.Clear();
        }

        bool CanClearGridCommand()
        {
            return SorterGenomeEvalVms.Any();
        }

        #endregion // ClearGridCommand

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
            sb.AppendLine
              (
                "SorterGenomeJson" + "\t" +
                "Success" + "\t" +
                "UseCount" + "\t" +
                "Ancestors" + "\t" +
                "SwitchableGroupGuid" + "\t" +
                "Switches"
              );
            foreach (var sorterGenomeEvalVm in SorterGenomeEvalVms)
            {
                sb.AppendLine
                (
                    sorterGenomeEvalVm.SorterGenomeJson + "\t" +
                    sorterGenomeEvalVm.Success + "\t" +
                    sorterGenomeEvalVm.SwitchUseCount + "\t" +
                    sorterGenomeEvalVm.Ancestors + "\t" +
                    sorterGenomeEvalVm.SwitchableGroupGuid + "\t" +
                    sorterGenomeEvalVm.SwitchUseList
                );
            }

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
            var vmList = new List<ISorterGenomeEvalVm>();
            try
            {
                var clipboardLines = Clipboard.GetText().Split("\n".ToCharArray());

                foreach (var line in clipboardLines.Where(t => !String.IsNullOrEmpty(t)))
                {
                    vmList.Add(line.ToSorterGenomeEvalVm());
                }
            }
            catch (Exception)
            {
                MessageBox.Show("cant parse data on clipboard");
                return;
            }
            SorterGenomeEvalVms.AddMany(vmList);
        }

        bool CanPasteGridCommand()
        {
            return Clipboard.ContainsText();
        }

        #endregion // PasteGridCommand

    }
}
