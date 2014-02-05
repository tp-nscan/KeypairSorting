using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using WpfUtils;

namespace KeypairSorting.ViewModels.MutateSorters
{
    public class SgMutantProfileGridVm : ViewModelBase
    {
        public SgMutantProfileGridVm(string name)
        {
            _name = name;
        }

        private readonly string _name;
        public string Name
        {
            get { return _name; }
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
            sb.AppendLine("Guid\tJson");

            foreach (var sorterVm in SgMutantProfileVms)
            {
                sb.AppendLine(sorterVm.ToString());
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
            var vmList = new List<ISgMutantProfileVm>();
            try
            {
                var clipboardLines = Clipboard.GetText().Split("\n".ToCharArray());

                foreach (var line in clipboardLines.Where(t => !String.IsNullOrEmpty(t)))
                {
                    vmList.Add(SgMutantProfileVm.ToSgMutantProfileVm(null));
                }
            }
            catch (Exception)
            {
                MessageBox.Show("cant parse data on clipboard");
                return;
            }
            SgMutantProfileVms.AddMany(vmList);
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
            SgMutantProfileVms.Clear();
        }

        bool CanClearGridCommand()
        {
            return SgMutantProfileVms.Any();
        }

        #endregion // ClearGridCommand

        private readonly ObservableCollection<ISgMutantProfileVm> _sgMutantProfileVms = new ObservableCollection<ISgMutantProfileVm>();
        public ObservableCollection<ISgMutantProfileVm> SgMutantProfileVms
        {
            get { return _sgMutantProfileVms; }
        }

    }
}
