using System.Collections.Generic;
using MathUtils.Rand;

namespace Genomic.Genes
{
    public interface IGeneUlongModN : IGene
    {
        ulong MaxVal { get; }
        ulong Val { get; }
    }

    public static class GeneUlongModN
    {
        public static IGeneUlongModN Make(ulong val, ulong maxVal)
        {
            return new GeneUlongModNImpl(val, maxVal);
        }

        public static IGeneUlongModN Make(IList<uint> val, ulong maxVal)
        {
            return new GeneUlongModNImpl(val, maxVal);
        }
    }

    class GeneUlongModNImpl : IGeneUlongModN
    {
        public GeneUlongModNImpl(ulong val, ulong maxVal)
        {
            _val = val;
            _maxVal = maxVal;
        }

        public GeneUlongModNImpl(IList<uint> val, ulong maxVal)
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
            return GeneUlongModN.Make(rando.NextUlong(MaxVal), MaxVal);
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
}