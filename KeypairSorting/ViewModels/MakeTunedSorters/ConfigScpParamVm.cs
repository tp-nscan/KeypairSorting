using System;
using SorterEvo.Workflows;
using WpfUtils;

namespace KeypairSorting.ViewModels.MakeTunedSorters
{
    public class ConfigScpParamVm : ViewModelBase
    {
        public ConfigScpParamVm(IScpParams scpParams)
        {
            Name = scpParams.Name;
            SorterLayerStartingGenomeCount = scpParams.SorterLayerStartingGenomeCount;
            SorterLayerExpandedGenomeCount = scpParams.SorterLayerExpandedGenomeCount;
            SorterMutationRate = scpParams.SorterMutationRate;
            SorterInsertionRate = scpParams.SorterInsertionRate;
            SorterDeletionRate = scpParams.SorterDeletionRate;
            Seed = scpParams.Seed;
            TotalGenerations = scpParams.TotalGenerations;
        }

        private string _name;
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

        private int? _sorterLayerStartingGenomeCount;
        public int? SorterLayerStartingGenomeCount
        {
            get { return _sorterLayerStartingGenomeCount; }
            set
            {
                _sorterLayerStartingGenomeCount = value;
                OnPropertyChanged("SorterLayerStartingGenomeCount");
            }
        }

        private int? _sorterLayerExpandedGenomeCount;
        public int? SorterLayerExpandedGenomeCount
        {
            get { return _sorterLayerExpandedGenomeCount; }
            set
            {
                _sorterLayerExpandedGenomeCount = value;
                OnPropertyChanged("SorterLayerExpandedGenomeCount");
            }
        }

        private double? _sorterMutationRate;
        public double? SorterMutationRate
        {
            get { return _sorterMutationRate; }
            set
            {
                _sorterMutationRate = value;
                OnPropertyChanged("SorterMutationRate");
            }
        }

        private double? _sorterInsertionRate;
        public double? SorterInsertionRate
        {
            get { return _sorterInsertionRate; }
            set
            {
                _sorterInsertionRate = value;
                OnPropertyChanged("SorterInsertionRate");
            }
        }

        private double? _sorterDeletionRate;
        public double? SorterDeletionRate
        {
            get { return _sorterDeletionRate; }
            set
            {
                _sorterDeletionRate = value;
                OnPropertyChanged("SorterDeletionRate");
            }
        }

        public bool HasValidData
        {
            get
            {
                return
                    !String.IsNullOrEmpty(Name)
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
