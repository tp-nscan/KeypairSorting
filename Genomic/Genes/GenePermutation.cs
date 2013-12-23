using System.Collections.Generic;
using System.Linq;
using MathUtils.Collections;
using MathUtils.Rand;

namespace Genomic.Genes
{
    public interface IGenePermutation : IGene
    {
        IReadOnlyList<uint> Items { get; }
    }

    public static class GenePermutation
    {
        public static IGenePermutation Make(IEnumerable<uint> items, double mixingRate)
        {
            return new GenePermutationImpl(items, mixingRate);
        }
    }

    class GenePermutationImpl : IGenePermutation
    {
        public GenePermutationImpl(IEnumerable<uint> items, double mixingRate)
        {
            _mixingRate = mixingRate;
            _items = items.ToList();
        }

        private readonly IReadOnlyList<uint> _items;
        public IReadOnlyList<uint> Items
        {
            get { return _items; }
        }

        public IEnumerable<uint> ToIntStream
        {
            get { return _items; }
        }

        public IGene Mutate(IRando rando)
        {
            return GenePermutation.Make(Items.FisherYatesPartialShuffle(rando, MixingRate), MixingRate);
        }

        private readonly double _mixingRate;
        public double MixingRate
        {
            get { return _mixingRate; }
        }
    }
}