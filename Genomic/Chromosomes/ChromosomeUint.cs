using System;
using System.Collections.Generic;
using System.Linq;
using Genomic.Genes;
using MathUtils.Rand;

namespace Genomic.Chromosomes
{
    public interface IChromosomeUint : IChromosome<IGeneUintModN>
    {
        uint MaxVal { get; }
    }

    public static class ChromosomeUint
    {
        public static IChromosomeUint Make
            (
                IReadOnlyList<uint> sequence,
                uint maxVal
            )
        {
            return new ChromosomeUintImpl
                (
                    sequence: sequence,
                    maxVal: maxVal
                );
        }
    }

    internal class ChromosomeUintImpl : ChromosomeImpl<IGeneUintModN>, IChromosomeUint
    {
        public ChromosomeUintImpl
            (
                IReadOnlyList<uint> sequence, 
                uint maxVal
            ) : base(sequence)
        {
            _maxVal = maxVal;
        }

        private readonly uint _maxVal;
        public uint MaxVal
        {
            get { return _maxVal; }
        }

        private IReadOnlyList<IGeneUintModN> _blockList;
        public override IReadOnlyList<IGeneUintModN> Blocks
        {
            get
            {
                return _blockList ?? 
                    (
                        _blockList = Sequence
                        .Select(t => GeneUintModN.Make(t, MaxVal))
                        .ToList()
                    );
            }
        }

        public override IGeneUintModN NewBlock(IRando rando)
        {
            return GeneUintModN.Make(rando.NextUint(MaxVal), MaxVal);
        }

        public override IChromosome<IGeneUintModN> Mutate(
            Func<IReadOnlyList<IGeneUintModN>, IReadOnlyList<IGeneUintModN>> mutator)
        {
            return new ChromosomeUintImpl
                (
                    mutator(Blocks).SelectMany(b => b.ToIntStream).ToList(),
                    MaxVal
                );
        }

        public override Tuple<IChromosome<IGeneUintModN>, IChromosome<IGeneUintModN>> Recombine(
            Func<
                 IReadOnlyList<IGeneUintModN>, IReadOnlyList<IGeneUintModN>,
                 Tuple<IReadOnlyList<IGeneUintModN>, IReadOnlyList<IGeneUintModN>>
                > recombinator,
            IReadOnlyList<IGeneUintModN> partner)
        {
            var children = recombinator(Blocks, partner);

            return new Tuple<IChromosome<IGeneUintModN>, IChromosome<IGeneUintModN>>(
                new ChromosomeUintImpl
                (
                    children.Item1.SelectMany(b => b.ToIntStream).ToList(),
                    MaxVal
                ),
                new ChromosomeUintImpl
                (
                    children.Item2.SelectMany(b => b.ToIntStream).ToList(),
                    MaxVal
                )
           );
        }

    }
}