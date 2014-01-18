﻿using System.Windows.Input;
using KeypairSorting.Resources;
using SorterEvo.Workflows;
using WpfUtils;

namespace KeypairSorting.ViewModels.MakeTunedSorters
{
    public class TunedSortersVm : ViewModelBase, IToolTemplateVm
    {
        public TunedSortersVm()
        {
            _configRunSelectorVm = new ConfigScpParamVm(
                    ScpParams.Make(
                        sorterLayerStartingGenomeCount: 10,
                        sorterLayerExpandedGenomeCount: 30,
                        sorterMutationRate: 0.03,
                        sorterInsertionRate: 0.03,
                        sorterDeletionRate: 0.03,
                        name: "standard",
                        seed: 1234,
                        totalGenerations: 10
                    ),
                    RunTunedSortersCommand
                );
        }

        public ToolTemplateType ToolTemplateType
        {
            get { return ToolTemplateType.SorterTune; }
        }

        public string Description
        {
            get { return "Optimize Sorters"; }
        }

        #region RunTunedSortersCommand

        RelayCommand _runTunedSortersCommand;
        public ICommand RunTunedSortersCommand
        {
            get
            {
                return _runTunedSortersCommand ?? (_runTunedSortersCommand
                    = new RelayCommand
                        (
                            param => OnRunTunedSortersCommand(),
                            param => CanRunTunedSortersCommand()
                        ));
            }
        }

        void OnRunTunedSortersCommand()
        {
            ConfigRunSelectorVm = new RunTunedSortersVm(((ConfigScpParamVm)ConfigRunSelectorVm).GetParams);
        }

        bool CanRunTunedSortersCommand()
        {
            var configSorterPoolParams = _configRunSelectorVm as ConfigScpParamVm;

            return (configSorterPoolParams==null) || configSorterPoolParams.HasValidData;
        }

        #endregion // RunTunedSortersCommand

        private IConfigRunSelectorVm _configRunSelectorVm;
        public IConfigRunSelectorVm ConfigRunSelectorVm
        {
            get { return _configRunSelectorVm; }
            set
            {
                _configRunSelectorVm = value;
                OnPropertyChanged("ConfigRunSelectorVm");
            }
        }
    }
}
