using System;
using SorterEvo.Genomes;
using SorterEvo.Json.Genomes;
using WpfUtils;

namespace KeypairSorting.ViewModels.Parts
{
    public class SorterGenomeVm : ViewModelBase
    {
        public SorterGenomeVm(ISorterGenome sorterGenome)
        {
            _sorterGenome = sorterGenome;
        }

        private readonly ISorterGenome _sorterGenome;

        public Guid Guid
        {
            get { return _sorterGenome.Guid; }
        }

        public string SorterGenomeJson
        {
            get { return _sorterGenome.ToJsonString(); }
        }

    }
}
