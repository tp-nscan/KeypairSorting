using System;
using System.Linq;
using System.Windows.Input;
using Genomic.Chromosomes;
using KeypairSorting.Resources;
using KeypairSorting.ViewModels.Parts;
using SorterEvo.Genomes;
using Sorting.KeyPairs;
using WpfUtils;

namespace KeypairSorting.ViewModels
{
    public class MakeSorterGenomesVm : ViewModelBase, IToolTemplateVm
    {
        public MakeSorterGenomesVm()
        {
            _sorterGenomeGridVm = new SorterGenomeGridVm();
            _sorterGridVm = new SorterGridVm();
        }

        public ToolTemplateType ToolTemplateType
        {
            get { return ToolTemplateType.SorterGenomeGen; }
        }

        public string Description
        {
            get { return "Convert Sorters to SorterGenomes"; }
        }

        private readonly SorterGenomeGridVm _sorterGenomeGridVm;
        public SorterGenomeGridVm SorterGenomeGridVm
        {
            get { return _sorterGenomeGridVm; }
        }

        private readonly SorterGridVm _sorterGridVm;
        public SorterGridVm SorterGridVm
        {
            get { return _sorterGridVm; }
        }

        #region ConvertToGenomesCommand

        RelayCommand _convertToGenomesCommand;
        public ICommand ConvertToGenomesCommand
        {
            get
            {
                return _convertToGenomesCommand ?? (_convertToGenomesCommand
                    = new RelayCommand
                        (
                            param => OnConvertToGenomesCommand(),
                            param => CanConvertToGenomesCommand()
                        ));
            }
        }

        protected void OnConvertToGenomesCommand()
        {
            foreach (var sorterVm in SorterGridVm.SorterVms)
            {
                SorterGenomeGridVm.SorterGenomeVms.Add
                    (
                        new SorterGenomeVm
                            (
                                SorterGenome.Make
                                (
                                    guid: sorterVm.Guid,
                                    parentGuid: Guid.Empty,
                                    chromosome: ChromosomeUint.Make
                                    (
                                        sequence: sorterVm.Sorter.KeyPairs.Select(kp=>(uint)kp.Index).ToArray(), 
                                        maxVal: (uint) KeyPairRepository.KeyPairSetSizeForKeyCount(sorterVm.Sorter.KeyCount)
                                    ),
                                    keyCount: sorterVm.Sorter.KeyCount,
                                    keyPairCount: sorterVm.Sorter.KeyPairCount
                                )
                            )
                    );
            }
        }

        bool CanConvertToGenomesCommand()
        {
            return true;
        }

        #endregion // ConvertToGenomesCommand

    }
}
