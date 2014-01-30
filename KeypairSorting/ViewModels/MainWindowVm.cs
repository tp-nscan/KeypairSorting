using System;
using System.Windows.Input;
using KeypairSorting.Resources;
using KeypairSorting.ViewModels.MutateSorters;
using KeypairSorting.ViewModels.Scp;
using WpfUtils;

namespace KeypairSorting.ViewModels
{
    public class MainWindowVm : ViewModelBase
    {
        public MainWindowVm()
        {

        }

        public IToolTemplateVm ToolTemplateVm
        {
            get { return _toolTemplateVm; }
            set
            {
                _toolTemplateVm = value;
                OnPropertyChanged("ToolTemplateVm");
            }
        }

        public string Mode
        {
            get { return "Random sampler"; }
        }


        #region RandomSortersCommand

        RelayCommand _randomSortersCommand;
        public ICommand RandomSortersCommand
        {
            get
            {
                return _randomSortersCommand ?? (_randomSortersCommand
                    = new RelayCommand
                        (
                            param => OnRandomSortersCommand(),
                            param => CanRandomSortersCommand()
                        ));
            }
        }

        protected void OnRandomSortersCommand()
        {
            ToolTemplateVm = new MakeRandomSorterVm();
        }

        bool CanRandomSortersCommand()
        {
            return true;
        }

        #endregion // RandomSortersCommand

        #region EvalSortersCommand

        RelayCommand _evalSortersCommand;
        public ICommand EvalSortersCommand
        {
            get
            {
                return _evalSortersCommand ?? (_evalSortersCommand
                    = new RelayCommand
                        (
                            param => OnEvalSortersCommand(),
                            param => CanEvalSortersCommand()
                        ));
            }
        }

        protected void OnEvalSortersCommand()
        {
            ToolTemplateVm = new MakeSorterEvalsVm();
        }

        bool CanEvalSortersCommand()
        {
            return true;
        }

        #endregion // EvalSortersCommand

        #region EvalSortersCommand

        RelayCommand _evalSorterGenomesCommand;
        public ICommand EvalSorterGenomesCommand
        {
            get
            {
                return _evalSorterGenomesCommand ?? (_evalSorterGenomesCommand
                    = new RelayCommand
                        (
                            param => OnEvalSorterGenomesCommand(),
                            param => CanEvalSorterGenomesCommand()
                        ));
            }
        }

        void OnEvalSorterGenomesCommand()
        {
            ToolTemplateVm = new MakeSorterGenomeEvalsVm();
        }

        bool CanEvalSorterGenomesCommand()
        {
            return true;
        }

        #endregion // EvalSortersCommand

        #region SortersSwitchesEvalCommand

        RelayCommand _sortersSwitchesEvalCommand;
        public ICommand SortersSwitchesEvalCommand
        {
            get
            {
                return _sortersSwitchesEvalCommand ?? (_sortersSwitchesEvalCommand
                    = new RelayCommand
                        (
                            param => OnSortersSwitchesEvalCommand(),
                            param => CanSortersSwitchesEvalCommand()
                        ));
            }
        }

        protected void OnSortersSwitchesEvalCommand()
        {
            ToolTemplateVm = new MakeSorterSwitchableEvalsVm();
        }

        bool CanSortersSwitchesEvalCommand()
        {
            return true;
        }

        #endregion // SortersSwitchesEvalCommand

        #region MakeSorterGenomesCommand

        RelayCommand _makeSorterGenomesCommand;
        public ICommand MakeSorterGenomesCommand
        {
            get
            {
                return _makeSorterGenomesCommand ?? (_makeSorterGenomesCommand
                    = new RelayCommand
                        (
                            param => OnGenomeGenCommand(),
                            param => CanGenomeGenCommand()
                        ));
            }
        }

        protected void OnGenomeGenCommand()
        {
            ToolTemplateVm = new MakeSorterGenomesVm();
        }

        bool CanGenomeGenCommand()
        {
            return true;
        }

        #endregion // SortersSwitchesEvalCommand

        #region MutateSorterGenomesCommand

        RelayCommand _mutateSorterGenomesCommand;
        public ICommand MutateSorterGenomesCommand
        {
            get
            {
                return _mutateSorterGenomesCommand ?? (_mutateSorterGenomesCommand
                    = new RelayCommand
                        (
                            param => OnMutateSorterGenomesCommand(),
                            param => CanMutateSorterGenomesCommand()
                        ));
            }
        }

        protected void OnMutateSorterGenomesCommand()
        {
            ToolTemplateVm = new MutateSortersVm();
        }

        bool CanMutateSorterGenomesCommand()
        {
            return true;
        }

        #endregion // SortersSwitchesEvalCommand

        #region MultiTuneSortersCommand

        RelayCommand _multiTuneSortersCommand;
        public ICommand MultiTuneSortersCommand
        {
            get
            {
                return _multiTuneSortersCommand ?? (_multiTuneSortersCommand
                    = new RelayCommand
                        (
                            param => OnMultiTuneSortersCommand(),
                            param => CanMultiTuneSortersCommand()
                        ));
            }
        }

        protected void OnMultiTuneSortersCommand()
        {
            ToolTemplateVm = new MultiScpVm();
        }

        bool CanMultiTuneSortersCommand()
        {
            return true;
        }

        #endregion // TuneSortersCommand

        #region RandomSwitchesCommand

        RelayCommand _randomSwitchesCommand;
        public ICommand RandomSwitchesCommand
        {
            get
            {
                return _randomSwitchesCommand ?? (_randomSwitchesCommand
                    = new RelayCommand
                        (
                            param => OnRandomSwitchesCommand(),
                            param => CanRandomSwitchesCommand()
                        ));
            }
        }

        protected void OnRandomSwitchesCommand()
        {
            ToolTemplateVm = new MakeRandomSwitchablesVm();
        }

        bool CanRandomSwitchesCommand()
        {
            return true;
        }

        #endregion // RandomSwitchesCommand

        #region TuneSortersCommand

        RelayCommand _tuneSortersCommand;
        public ICommand TuneSortersCommand
        {
            get
            {
                return _tuneSortersCommand ?? (_tuneSortersCommand
                    = new RelayCommand
                        (
                            param => OnTuneSortersCommand(),
                            param => CanTuneSortersCommand()
                        ));
            }
        }

        protected void OnTuneSortersCommand()
        {
            ToolTemplateVm = new ScpVm();
        }

        bool CanTuneSortersCommand()
        {
            return true;
        }

        #endregion // TuneSortersCommand

        #region CloseCommand

        private RelayCommand _closeCommand;
        private IToolTemplateVm _toolTemplateVm;

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
