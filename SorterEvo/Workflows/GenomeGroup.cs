using System;
using System.Collections.Generic;
using System.Linq;
using Genomic.Genomes;
using MathUtils.Rand;

namespace SorterEvo.Workflows
{
    public interface IGenomeGroup<TG> where TG : IGenome
    {
        Guid Guid { get; }
    }

    public static class GenomeGroup
    {

    }

    public class GenomeGroup<TG> where TG : IGenome
    {

    }

    public class IntGen
    {
        private readonly int _seed;
        private readonly IReadOnlyList<int> _values;

        public IntGen(int seed, int count)
        {
            _seed = seed;
            _values = Rando.Fast(seed).ToIntEnumerator().Take(count).ToList();
        }

        public int Seed
        {
            get { return _seed; }
        }

        public IReadOnlyList<int> Values
        {
            get { return _values; }
        }
    }

    public class Summer
    {
        private readonly IReadOnlyList<int> _values;

        public Summer(IEnumerable<int> values)
        {
            _values = values.ToList();
        }

        public int Sum
        {
            get { return _values.Sum(t=>t); }
        }

        public IReadOnlyList<int> Values
        {
            get { return _values; }
        }
    }

    public class Combo
    {
        private readonly int _seed;
        private readonly int _count;
        private readonly IntGen _intGen;
        private readonly Summer _summer;

        public Combo(int seed, int count)
        {
            _seed = seed;
            _count = count;

            _intGen = new IntGen(Seed, Count);
            _summer = new Summer(IntGen.Values);
        }

        public int Seed
        {
            get { return _seed; }
        }

        public int Count
        {
            get { return _count; }
        }

        private IntGen IntGen
        {
            get { return _intGen; }
        }

        private Summer Summer
        {
            get { return _summer; }
        }

        public int Result
        {
            get { return Summer.Sum; }
        }
    }
}
