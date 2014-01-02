using System;
using System.Windows.Input;
using WpfUtils;

namespace KeypairSorting.ViewModels
{
    public class MainWindowVm : ViewModelBase
    {

        public MainWindowVm()
        {
            //_sorterRandomSamplerVm = new SorterRandomSamplerVm();
           // _sorterEvalVm = new SorterEvalVm();
            //_makeSwitchableGroupsVm = new MakeSwitchableGroupsVm();
            _makeSorterOnSwitchableGroupVms = new MakeSorterOnSwitchableGroupsVm();
        }

        //private readonly SorterRandomSamplerVm _sorterRandomSamplerVm;
        //public SorterRandomSamplerVm SorterRandomSamplerVm
        //{
        //    get { return _sorterRandomSamplerVm; }
        //}

        //private readonly SorterEvalVm _sorterEvalVm;
        //public SorterEvalVm SorterEvalVm
        //{
        //    get { return _sorterEvalVm; }
        //}

        //private readonly MakeSwitchableGroupsVm _makeSwitchableGroupsVm;
        //public MakeSwitchableGroupsVm MakeSwitchableGroupsVm
        //{
        //    get { return _makeSwitchableGroupsVm; }
        //}


        private readonly MakeSorterOnSwitchableGroupsVm _makeSorterOnSwitchableGroupVms;
        public MakeSorterOnSwitchableGroupsVm MakeSorterOnSwitchableGroupVms
        {
            get { return _makeSorterOnSwitchableGroupVms; }
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
