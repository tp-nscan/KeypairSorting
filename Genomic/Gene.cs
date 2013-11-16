using System;
using System.Collections.Generic;
using System.Linq;
using MathUtils.Rand;

namespace Genomic
{
    public interface IGene
    {
        IEnumerable<uint> AsSerialized { get; }
        IGene Mutate(IRando rando);
    }

    public class GeneUintModN : IGene
    {
        public GeneUintModN(uint val, uint maxVal)
        {
            if (val >= maxVal)
            {
                throw new ArgumentException("val must be less than maxval");
            }
            _val = val;
            _maxVal = maxVal;
        }

        public IEnumerable<uint> AsSerialized
        {
            get { yield return _val; }
        }

        public IGene Mutate(IRando rando)
        {
            return new GeneUintModN(rando.NextUint(MaxVal), MaxVal);
        }

        private readonly uint _maxVal;
        public uint MaxVal
        {
            get { return _maxVal; }
        }

        private readonly uint _val;
        public uint Val
        {
            get { return _val; }
        }
    }

    public class GeneUlongModN : IGene
    {
        public GeneUlongModN(ulong val, ulong maxVal)
        {
            _val = val;
            _maxVal = maxVal;
        }

        public GeneUlongModN(IList<uint> val, ulong maxVal)
        {
            _val = ((ulong)val[0] << 32) + val[1];
            _maxVal = maxVal;
        }

        public IEnumerable<uint> AsSerialized
        {
            get
            {
                yield return (uint)(_val >> 32);
                yield return (uint)_val;
            }
        }

        public IGene Mutate(IRando rando)
        {
            return new GeneUlongModN(rando.NextUlong(MaxVal), MaxVal);
        }

        private readonly ulong _maxVal;
        public ulong MaxVal
        {
            get { return _maxVal; }
        }

        private readonly ulong _val;
        public ulong Val
        {
            get { return _val; }
        }
    }

    public class GenePermutationN : IGene
    {
        public GenePermutationN(IEnumerable<uint> items)
        {
            _permutation = items.ToList();
        }

        private readonly IReadOnlyList<uint> _permutation;
        public IReadOnlyList<uint> Permutation
        {
            get { return _permutation; }
        }

        public IEnumerable<uint> AsSerialized
        {
            get { return _permutation; }
        }

        public IGene Mutate(IRando rando)
        {
            return new GenePermutationN(rando.FisherYatesShuffle(Permutation));
        }
    }
}
