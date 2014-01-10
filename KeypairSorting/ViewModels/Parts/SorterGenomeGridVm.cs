using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using SorterEvo.Json.Genomes;
using WpfUtils;

namespace KeypairSorting.ViewModels.Parts
{
    public class SorterGenomeGridVm : ViewModelBase
    {
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
            SorterGenomeVms.Clear();
        }

        bool CanClearGridCommand()
        {
            return SorterGenomeVms.Any();
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
            sb.AppendLine("Guid\tJson");

            foreach (var sorterGenomeVm in SorterGenomeVms)
            {
                sb.AppendLine(sorterGenomeVm.Guid + "\t" + sorterGenomeVm.SorterGenomeJson);
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
            var vmList = new List<SorterGenomeVm>();
            try
            {
                var clipboardLines = Clipboard.GetText().Split("\n".ToCharArray());

                foreach (var line in clipboardLines.Where(t => !String.IsNullOrEmpty(t)))
                {
                    vmList.Add(new SorterGenomeVm(line.ToSorterGenome()));
                }
            }
            catch (Exception)
            {
                MessageBox.Show("cant parse data on clipboard");
                return;
            }
            SorterGenomeVms.AddMany(vmList);
        }

        bool CanPasteGridCommand()
        {
            return Clipboard.ContainsText();
        }

        #endregion // PasteGridCommand

        private readonly ObservableCollection<SorterGenomeVm> _sorterGenomeVms = new ObservableCollection<SorterGenomeVm>();
        public ObservableCollection<SorterGenomeVm> SorterGenomeVms
        {
            get { return _sorterGenomeVms; }
        }
    }
}
