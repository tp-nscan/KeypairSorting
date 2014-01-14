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
                    name: sorterCompPoolParamsVm.Name
                );
        }
    }

    public class SorterCompPoolParamsVm : ViewModelBase
    {
        private string _name;
        private int? _sorterLayerStartingGenomeCount;
        private int? _sorterLayerExpandedGenomeCount;
        private double? _sorterMutationRate;
        private double? _sorterInsertionRate;
        private double? _sorterDeletionRate;

        public SorterCompPoolParamsVm(ISorterCompPoolParams sorterCompPoolParams)
        {
            Name = sorterCompPoolParams.Name;
            SorterLayerStartingGenomeCount = sorterCompPoolParams.SorterLayerStartingGenomeCount;
            SorterLayerExpandedGenomeCount = sorterCompPoolParams.SorterLayerExpandedGenomeCount;
            SorterMutationRate = sorterCompPoolParams.SorterMutationRate;
            SorterInsertionRate = sorterCompPoolParams.SorterInsertionRate;
            SorterDeletionRate = sorterCompPoolParams.SorterDeletionRate;
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
                    SorterMutationRate.HasValue;
            }
        }

        public ISorterCompPoolParams GetParams
        {
            get
            {

                return HasValidData ?
                        SorterCompPoolParams.Make
                        (
                            // ReSharper disable PossibleInvalidOperationException
                            sorterLayerStartingGenomeCount: SorterLayerStartingGenomeCount.Value,
                            sorterLayerExpandedGenomeCount: SorterLayerExpandedGenomeCount.Value,
                            sorterMutationRate: SorterMutationRate.Value,
                            sorterInsertionRate: SorterInsertionRate.Value,
                            sorterDeletionRate: SorterDeletionRate.Value,
                            // ReSharper restore PossibleInvalidOperationException
                            name: Name
                        )
                        :
                        null;
            }
        }
    }
}
