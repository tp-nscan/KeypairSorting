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
    public class SorterEvalsGridVm : ViewModelBase
    {
        private ObservableCollection<ISorterEvalVm> _sorterEvalVms
                = new ObservableCollection<ISorterEvalVm>();

        public ObservableCollection<ISorterEvalVm> SorterEvalVms
        {
            get { return _sorterEvalVms; }
            set { _sorterEvalVms = value; }
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
            if (CopyHeaders)
            {
                sb.AppendLine
                    (
                        "SorterJson" + "\t" +
                        "Success" + "\t" +
                        "UseCount" + "\t" +
                        "SwitchableGuid" + "\t" +
                        "Switches"
                    );
            }
            foreach (var sorterEvalVm in SorterEvalVms)
            {
                sb.AppendLine
                    (
                        sorterEvalVm.SorterJson + "\t" +
                        sorterEvalVm.Success + "\t" +
                        sorterEvalVm.UseCount + "\t" +
                        sorterEvalVm.SwitchableGuid + "\t" +
                        sorterEvalVm.SwitchUseList
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

        RelayCommand _copyResultsCommand;
        public ICommand PasteGridCommand
        {
            get
            {
                return _copyResultsCommand ?? (_copyResultsCommand
                    = new RelayCommand
                        (
                            param => OnPasteGridCommand(),
                            param => CanPasteGridCommand()
                        ));
            }
        }

        protected void OnPasteGridCommand()
        {
            var vmList = new List<ISorterEvalVm>();
            try
            {
                var clipboardLines = Clipboard.GetText().Split("\n".ToCharArray());

                foreach (var line in clipboardLines.Where(t => !String.IsNullOrEmpty(t)))
                {
                    vmList.Add(line.ToSorterEvalVm());
                }
            }
            catch (Exception)
            {
                MessageBox.Show("cant parse data on clipboard");
                return;
            }

            SorterEvalVms.AddMany(vmList);
        }

        bool CanPasteGridCommand()
        {
            return true;
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
            SorterEvalVms.Clear();
        }

        bool CanClearGridCommand()
        {
            return SorterEvalVms.Any();
        }

        #endregion // ClearGridCommand

        private bool _copyHeaders;
        public bool CopyHeaders
        {
            get { return _copyHeaders; }
            set
            {
                _copyHeaders = value;
                OnPropertyChanged("CopyHeaders");
            }
        }
    }
}
