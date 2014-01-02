using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using KeypairSorting.ViewModels;
using Sorting.Json.Sorters;
using Sorting.Json.Switchables;
using Sorting.Switchables;
using WpfUtils;

namespace KeypairSorting.ViewModels
{
    public class SwitchableGroupGridVm : ViewModelBase
    {
        private readonly ObservableCollection<SwitchableGroupVm> _switchableGroupVms = new ObservableCollection<SwitchableGroupVm>();

        public ObservableCollection<SwitchableGroupVm> SwitchableGroupVms
        {
            get { return _switchableGroupVms; }
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

            foreach (var switchableGroupVm in SwitchableGroupVms)
            {
                sb.AppendLine(switchableGroupVm.SwitchableGroupJson);
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
            var vmList = new List<SwitchableGroupVm>();
            try
            {
                var clipboardLines = Clipboard.GetText().Split("\n".ToCharArray());

                foreach (var line in clipboardLines.Where(t => !String.IsNullOrEmpty(t)))
                {
                    vmList.Add(new SwitchableGroupVm(line.ToSwitchableGroup()));
                }
            }
            catch (Exception)
            {
                MessageBox.Show("cant parse data on clipboard");
                return;
            }

            SwitchableGroupVms.AddMany(vmList);
        }

        bool CanPasteGridCommand()
        {
            return Clipboard.ContainsText();
        }

        #endregion // PasteGridCommand


    }

}
