using System;
using System.Collections.Generic;
using MathUtils.Rand;

namespace Genomic.Genes
{
    public interface IGeneUintModN : IGene
    {
        uint MaxVal { get; }
        uint Val { get; }
    }

    public static class GeneUintModN
    {
        public static IGeneUintModN Make(uint val, uint maxVal)
        {
            return new GeneUintModNImpl(val, maxVal);
        }
    }

    class GeneUintModNImpl : IGeneUintModN
    {
        public GeneUintModNImpl(uint val, uint maxVal)
        {
            if (val >= maxVal)
            {
                throw new ArgumentException("val must be less than maxval");
            }
            _val = val;
            _maxVal = maxVal;
        }

        public IEnumerable<uint> ToIntStream
        {
            get { yield return _val; }
        }

        public IGene Mutate(IRando rando)
        {
            return GeneUintModN.Make(rando.NextUint(MaxVal), MaxVal);
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
}