using System;
using System.Text;
using System.Windows;
using System.Windows.Input;
using SorterEvo.Workflows;
using WpfUtils;

namespace KeypairSorting.ViewModels.Parts
{
    public static class SorterCompPoolParamsVmExt
    {
        public static ISorterCompPoolParams ToSorterCompPoolParams(this ISorterCompPoolParams sorterCompPoolParamsVm)
        {
            return SorterCompPoolParams.Make
                (
                    sorterLayerStartingGenomeCount: sorterCompPoolParamsVm.SorterLayerStartingGenomeCount,
                    sorterLayerExpandedGenomeCount: sorterCompPoolParamsVm.SorterLayerExpandedGenomeCount,
                    sorterMutationRate: sorterCompPoolParamsVm.SorterMutationRate,
                    sorterInsertionRate: sorterCompPoolParamsVm.SorterInsertionRate,
                    sorterDeletionRate: sorterCompPoolParamsVm.SorterDeletionRate,
                    name: sorterCompPoolParamsVm.Name,
                    seed: sorterCompPoolParamsVm.Seed,
                    totalGenerations: sorterCompPoolParamsVm.TotalGenerations
                );
        }
    }

    public class SorterCompPoolParamsVm : ViewModelBase, ISorterCompPoolParams
    {
        private string _name;
        private readonly int _sorterLayerStartingGenomeCount;
        private readonly int _sorterLayerExpandedGenomeCount;
        private readonly double _sorterMutationRate;
        private readonly double _sorterInsertionRate;
        private readonly double _sorterDeletionRate;

        public SorterCompPoolParamsVm(ISorterCompPoolParams sorterCompPoolParams)
        {
            Name = sorterCompPoolParams.Name;
            _sorterLayerStartingGenomeCount = sorterCompPoolParams.SorterLayerStartingGenomeCount;
            _sorterLayerExpandedGenomeCount = sorterCompPoolParams.SorterLayerExpandedGenomeCount;
            _sorterMutationRate = sorterCompPoolParams.SorterMutationRate;
            _sorterInsertionRate = sorterCompPoolParams.SorterInsertionRate;
            _sorterDeletionRate = sorterCompPoolParams.SorterDeletionRate;
            _totalGenerations = sorterCompPoolParams.TotalGenerations;
            _seed = sorterCompPoolParams.Seed;
            _currentGeneration = 0;
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

        public int SorterLayerStartingGenomeCount
        {
            get { return _sorterLayerStartingGenomeCount; }
        }

        public int SorterLayerExpandedGenomeCount
        {
            get { return _sorterLayerExpandedGenomeCount; }
        }

        public double SorterMutationRate
        {
            get { return _sorterMutationRate; }
        }

        public double SorterInsertionRate
        {
            get { return _sorterInsertionRate; }
        }

        public double SorterDeletionRate
        {
            get { return _sorterDeletionRate; }
        }


        private readonly int _seed;
        public int Seed
        {
            get { return _seed; }
        }

        private int _currentGeneration;
        public int CurrentGeneration
        {
            get { return _currentGeneration; }
            set
            {
                _currentGeneration = value;
                OnPropertyChanged("CurrentGeneration");
            }
        }

        private readonly int _totalGenerations;
        public int TotalGenerations
        {
            get { return _totalGenerations; }
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
            sb.AppendLine("Starting Count\t" + SorterLayerStartingGenomeCount);
            sb.AppendLine("Expanded Count\t" + SorterLayerExpandedGenomeCount);
            sb.AppendLine("Deletion rate\t" + SorterDeletionRate.ToString("0.000"));
            sb.AppendLine("Insertion rate\t" + SorterInsertionRate.ToString("0.000"));
            sb.AppendLine("Mutation rate\t" + SorterMutationRate.ToString("0.000"));
            sb.AppendLine("Seed\t" + SorterMutationRate.ToString("0.000"));
            sb.AppendLine("Current generation\t" + CurrentGeneration.ToString("0.000"));
            sb.AppendLine("Total generatinos\t" + TotalGenerations.ToString("0.000"));

            Clipboard.SetText(sb.ToString());
        }

        bool CanCopyCommand()
        {
            return true;
        }

        #endregion // CopyCommand


        public ISorterCompPoolParams GetParams
        {
            get
            {
                return SorterCompPoolParams.Make
                    (
                        // ReSharper disable PossibleInvalidOperationException
                        sorterLayerStartingGenomeCount: SorterLayerStartingGenomeCount,
                        sorterLayerExpandedGenomeCount: SorterLayerExpandedGenomeCount,
                        sorterMutationRate: SorterMutationRate,
                        sorterInsertionRate: SorterInsertionRate,
                        sorterDeletionRate: SorterDeletionRate,
                        seed: Seed,
                        totalGenerations: CurrentGeneration,
                        // ReSharper restore PossibleInvalidOperationException
                        name: Name
                    );
            }
        }
    }
}
