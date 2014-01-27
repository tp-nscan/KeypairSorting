using System;
using System.Collections.Generic;
using System.Linq;
using Genomic.Genes;
using MathUtils.Collections;
using MathUtils.Rand;

namespace Genomic.Chromosomes
{
    public interface IChromosomeUlongN : IChromosome<IGeneUlongModN>
    {
        ulong MaxVal { get; }
    }

    public static class ChromosomeUlongN
    {
        public static IChromosomeUlongN Make
            (
                Guid guid,
                IReadOnlyList<uint> sequence,
                ulong maxVal
            )
        {
            return new ChromosomeUlongNImpl
                (
                    sequence: sequence,
                    maxVal: maxVal
                );
        }
    }

    internal class ChromosomeUlongNImpl : ChromosomeImpl<IGeneUlongModN>, IChromosomeUlongN
    {
        public ChromosomeUlongNImpl(
            IReadOnlyList<uint> sequence,
            ulong maxVal
            ) : base(sequence)
        {
            _maxVal = maxVal;
        }

        private readonly ulong _maxVal;
        public ulong MaxVal
        {
            get { return _maxVal; }
        }

        private IReadOnlyList<IGeneUlongModN> _blockList;
        public override IReadOnlyList<IGeneUlongModN> Blocks
        {
            get
            {
                return _blockList ??
                       (
                           _blockList = Sequence.ToUlongs()
                               .Select(ul => GeneUlongModN.Make(ul, MaxVal))
                                .ToList()
                       );
            }
        }

        public override IGeneUlongModN NewBlock(IRando rando)
        {
            return GeneUlongModN.Make(rando.NextUlong(MaxVal), MaxVal);
        }

        public override IChromosome<IGeneUlongModN> Mutate(Func<IReadOnlyList<IGeneUlongModN>, IReadOnlyList<IGeneUlongModN>> mutator)
        {
            return new ChromosomeUlongNImpl
                (
                    mutator(Blocks).SelectMany(b => b.ToIntStream).ToList(),
                    MaxVal
                );
        }

        public override Tuple<IChromosome<IGeneUlongModN>, IChromosome<IGeneUlongModN>> Recombine(
            Func<
                 IReadOnlyList<IGeneUlongModN>, IReadOnlyList<IGeneUlongModN>,
                 Tuple<IReadOnlyList<IGeneUlongModN>, IReadOnlyList<IGeneUlongModN>>
                > recombinator,
            IReadOnlyList<IGeneUlongModN> partner)
        {
            var children = recombinator(Blocks, partner);

            return new Tuple<IChromosome<IGeneUlongModN>, IChromosome<IGeneUlongModN>>(
                new ChromosomeUlongNImpl
                (
                    children.Item1.SelectMany(b => b.ToIntStream).ToList(),
                    MaxVal
                ),
                new ChromosomeUlongNImpl
                (
                    children.Item1.SelectMany(b => b.ToIntStream).ToList(),
                    MaxVal
                )
           );
        }

    }
}