using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Genomic;

namespace SorterEvo
{
    public interface ISwitchableGroupGenome : IGenome
    {
        int KeyCount { get; }
    }

    public static class SwitchableGroupGenome 
    {

    }

    class SwitchableGroupGenomeImpl<T> : ISwitchableGroupGenome
    {
        private int _keyCount;
        public int KeyCount
        {
            get { return _keyCount; }
        }

        private IReadOnlyList<IChromosome> _chromosomes;

        public IReadOnlyList<IChromosome> Chromosomes
        {
            get { return _chromosomes; }
        }

        private Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
        }
    }
}
