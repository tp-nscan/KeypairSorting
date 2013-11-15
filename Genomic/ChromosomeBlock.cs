using System.Collections.Generic;
using MathUtils.Rand;

namespace Genomic
{
    public interface IChromosomeBlock
    {
        IEnumerable<uint> AsSerialized { get; }
        IChromosomeBlock Mutate(IRando rando);
    }

    public class ModNBlock : IChromosomeBlock
    {
        public ModNBlock(uint val, uint maxVal)
        {
            _val = val;
            _maxVal = maxVal;
        }

        public IEnumerable<uint> AsSerialized
        {
            get { yield return _val; }
        }

        public IChromosomeBlock Mutate(IRando rando)
        {
            return new ModNBlock(rando.NextUint(MaxVal), MaxVal);
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
