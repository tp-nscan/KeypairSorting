using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfUtils;

namespace KeypairSorting.ViewModels
{
    public class MainWindowVm : ViewModelBase
    {
        private readonly SorterRandomSamplerVm _sorterRandomSamplerVm;

        public MainWindowVm()
        {
            _sorterRandomSamplerVm = new SorterRandomSamplerVm();
        }

        public SorterRandomSamplerVm SorterRandomSamplerVm
        {
            get { return _sorterRandomSamplerVm; }
        }

        public string Mode
        {
            get { return "Random sampler"; }
        }


        #region CloseCommand

        private RelayCommand _closeCommand;
        /// <summary>
        /// Returns the command that, when invoked, attempts
        /// to remove this workspace from the user interface.
        /// </summary>
        public ICommand CloseCommand
        {
            get
            {
                if (_closeCommand == null)
                    _closeCommand = new RelayCommand(param => OnRequestClose());

                return _closeCommand;
            }
        }

        #endregion // CloseCommand


        #region RequestClose [event]

        /// <summary>
        /// Raised when this workspace should be removed from the UI.
        /// </summary>
        public event EventHandler RequestClose;

        private void OnRequestClose()
        {
            EventHandler handler = RequestClose;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        #endregion // RequestClose [event]
    }
}
