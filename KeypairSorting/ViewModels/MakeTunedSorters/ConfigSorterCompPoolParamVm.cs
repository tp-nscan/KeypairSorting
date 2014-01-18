using System;
using System.Text;
using System.Windows;
using System.Windows.Input;
using KeypairSorting.Resources;
using SorterEvo.Workflows;
using WpfUtils;

namespace KeypairSorting.ViewModels.MakeTunedSorters
{
    public class ConfigScpParamVm : ViewModelBase, IConfigRunSelectorVm
    {
        private string _name;
        private int? _sorterLayerStartingGenomeCount;
        private int? _sorterLayerExpandedGenomeCount;
        private double? _sorterMutationRate;
        private double? _sorterInsertionRate;
        private double? _sorterDeletionRate;

        public ConfigScpParamVm(IScpParams scpParams, ICommand runTunedSortersCommand)
        {
            RunTunedSortersCommand = runTunedSortersCommand;
            Name = scpParams.Name;
            SorterLayerStartingGenomeCount = scpParams.SorterLayerStartingGenomeCount;
            SorterLayerExpandedGenomeCount = scpParams.SorterLayerExpandedGenomeCount;
            SorterMutationRate = scpParams.SorterMutationRate;
            SorterInsertionRate = scpParams.SorterInsertionRate;
            SorterDeletionRate = scpParams.SorterDeletionRate;
            Seed = scpParams.Seed;
            TotalGenerations = scpParams.TotalGenerations;
        }

        public ICommand RunTunedSortersCommand { get; private set; }

        public ConfigRunTemplateType ConfigRunTemplateType
        {
            get { return ConfigRunTemplateType.Config; }
        }

        public string Description
        {
            get { return "Config sorter tune"; }
        }

        public String Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        private int? _seed;
        public int? Seed
        {
            get { return _seed; }
            set
            {
                _seed = value;
                OnPropertyChanged("Seed");
            }
        }

        private int? _totalGenerations;
        public int? TotalGenerations
        {
            get { return _totalGenerations; }
            set
            {
                _totalGenerations = value;
                OnPropertyChanged("TotalGenerations");
            }
        }

        public int? SorterLayerStartingGenomeCount
        {
            get { return _sorterLayerStartingGenomeCount; }
            set
            {
                _sorterLayerStartingGenomeCount = value;
                OnPropertyChanged("SorterLayerStartingGenomeCount");
            }
        }

        public int? SorterLayerExpandedGenomeCount
        {
            get { return _sorterLayerExpandedGenomeCount; }
            set
            {
                _sorterLayerExpandedGenomeCount = value;
                OnPropertyChanged("SorterLayerExpandedGenomeCount");
            }
        }

        public double? SorterMutationRate
        {
            get { return _sorterMutationRate; }
            set
            {
                _sorterMutationRate = value;
                OnPropertyChanged("SorterMutationRate");
            }
        }

        public double? SorterInsertionRate
        {
            get { return _sorterInsertionRate; }
            set
            {
                _sorterInsertionRate = value;
                OnPropertyChanged("SorterInsertionRate");
            }
        }

        public double? SorterDeletionRate
        {
            get { return _sorterDeletionRate; }
            set
            {
                _sorterDeletionRate = value;
                OnPropertyChanged("SorterDeletionRate");
            }
        }


        #region CopyCommand

        RelayCommand _copyCommand;
        public ICommand CopyCommand
        {
            get
            {
                return _copyCommand ?? (_copyCommand
                    = new RelayCommand
                        (
                            param => OnCopyCommand(),
                            param => CanCopyCommand()
                        ));
            }
        }

        protected void OnCopyCommand()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Name\t" + Name);
            // ReSharper disable PossibleInvalidOperationException
            sb.AppendLine("Starting Count\t" + SorterLayerStartingGenomeCount.Value);
            sb.AppendLine("Expanded Count\t" + SorterLayerExpandedGenomeCount.Value);
            sb.AppendLine("Deletion rate\t" + SorterDeletionRate.Value.ToString("0.000"));
            sb.AppendLine("Insertion rate\t" + SorterInsertionRate.Value.ToString("0.000"));
            sb.AppendLine("Mutation rate\t" + SorterMutationRate.Value.ToString("0.000"));
            // ReSharper restore PossibleInvalidOperationException

            Clipboard.SetText(sb.ToString());
        }

        bool CanCopyCommand()
        {
            return HasValidData;
        }

        #endregion // CopyCommand

        public bool HasValidData
        {
            get
            {
                return 
                    ! String.IsNullOrEmpty(Name) 
                    &&
                    SorterLayerStartingGenomeCount.HasValue 
                    &&
                    SorterLayerExpandedGenomeCount.HasValue 
                    &&
                    SorterDeletionRate.HasValue 
                    &&
                    SorterInsertionRate.HasValue 
                    &&
                    SorterMutationRate.HasValue
                    &&
                    TotalGenerations.HasValue
                    &&
                    Seed.HasValue;
            }
        }

        public IScpParams GetParams
        {
            get
            {

                return HasValidData ?
                        ScpParams.Make
                        (
                            // ReSharper disable PossibleInvalidOperationException
                            sorterLayerStartingGenomeCount: SorterLayerStartingGenomeCount.Value,
                            sorterLayerExpandedGenomeCount: SorterLayerExpandedGenomeCount.Value,
                            sorterMutationRate: SorterMutationRate.Value,
                            sorterInsertionRate: SorterInsertionRate.Value,
                            sorterDeletionRate: SorterDeletionRate.Value,
                            totalGenerations: TotalGenerations.Value,
                            seed: Seed.Value,
                            // ReSharper restore PossibleInvalidOperationException
                            name: Name
                        )
                        :
                        null;
            }
        }
    }


}
