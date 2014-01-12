using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Sorting.Json.Sorters;
using WpfUtils;

namespace KeypairSorting.ViewModels.Parts
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
            sb.AppendLine("Guid\tJson");

            foreach (var sorterVm in SorterVms)
            {
                sb.AppendLine(sorterVm.Guid + "\t" + sorterVm.SorterJson);
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
            var vmList = new List<SorterVm>();
            try
            {
                var clipboardLines = Clipboard.GetText().Split("\n".ToCharArray());

                foreach(var line in clipboardLines.Where( t=> ! String.IsNullOrEmpty(t)) )
                {
                    vmList.Add(new SorterVm(line.ToSorter()));
                }
            }
            catch (Exception)
            {
                MessageBox.Show("cant parse data on clipboard");
                return;
            }
            SorterVms.AddMany(vmList);
        }

        bool CanPasteGridCommand()
        {
            return Clipboard.ContainsText();
        }

        #endregion // PasteGridCommand

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
            SorterVms.Clear();
        }

        bool CanClearGridCommand()
        {
            return SorterVms.Any();
        }

        #endregion // ClearGridCommand

        private readonly ObservableCollection<SorterVm> _sorterVms = new ObservableCollection<SorterVm>();
        public ObservableCollection<SorterVm> SorterVms
        {
            get { return _sorterVms; }
        }

    }
}
