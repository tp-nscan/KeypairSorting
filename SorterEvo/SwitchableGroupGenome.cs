using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Evo.GenomeBuilders;
using Evo.Genomes;

namespace SorterEvo
{
    public interface ISwitchableGroupGenome<T> : IGenome<T>
    {
        int KeyCount { get; }
    }

    public static class SwitchableGroupGenome 
    {

    }

    class SwitchableGroupGenomeImpl<T> : ISwitchableGroupGenome<T>
    {
        private int _keyCount;
        private IReadOnlyList<IChromosome<T>> _chromosomes;
        private IGenomeBuildInfo _genomeBuildInfo;
        private Guid _guid;

        public int KeyCount
        {
            get { return _keyCount; }
        }

        public IReadOnlyList<IChromosome<T>> Chromosomes
        {
            get { return _chromosomes; }
        }

        public IGenomeBuildInfo GenomeBuildInfo
        {
            get { return _genomeBuildInfo; }
        }

        public Guid Guid
        {
            get { return _guid; }
        }
    }
}
