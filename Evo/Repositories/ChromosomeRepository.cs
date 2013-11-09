using System;
using System.Collections.Generic;
using System.Linq;
using Evo.GenomeBuilders;
using Evo.Genomes;

namespace Evo.Repositories
{
    public interface IChromosomeRepository
    {
        void AddChromosome(IChromosome chromosome);
        IChromosome GetChromosome(Guid id);
        IEnumerable<IChromosome> Chromosomes { get; }
    }

    public static class ChromosomeRepository
    {
        public static IChromosomeRepository WithTestData()
        {
            return new ChromosomeRepositoryImpl(

                new []
                {
                    ChromosomeBuildInfo.GenInfo(Guid.NewGuid(), 123, 700, 60).ToChromosome<int>(),
                    ChromosomeBuildInfo.GenInfo(Guid.NewGuid(), 124, 700, 60).ToChromosome<int>(),
                    ChromosomeBuildInfo.GenInfo(Guid.NewGuid(), 125, 700, 60).ToChromosome<int>(),
                    ChromosomeBuildInfo.GenInfo(Guid.NewGuid(), 126, 700, 60).ToChromosome<int>(),
                    ChromosomeBuildInfo.GenInfo(Guid.NewGuid(), 127, 700, 60).ToChromosome<int>(),
                    ChromosomeBuildInfo.GenInfo(Guid.NewGuid(), 128, 700, 60).ToChromosome<int>()                }
                
                );
        }
    }

    public class ChromosomeRepositoryImpl : IChromosomeRepository
    {
        readonly Dictionary<Guid, IChromosome> _chromosomes = new Dictionary<Guid, IChromosome>();

        public ChromosomeRepositoryImpl(IEnumerable<IChromosome> chromosomes)
        {
            _chromosomes = chromosomes.ToDictionary(c => c.Id);
        }

        public void AddChromosome(IChromosome chromosome)
        {
            _chromosomes[chromosome.Id] = chromosome;
        }

        public IChromosome GetChromosome(Guid id)
        {
           return _chromosomes[id];
        }

        public IEnumerable<IChromosome> Chromosomes
        {
            get { return _chromosomes.Values; }
        }
    }
}
