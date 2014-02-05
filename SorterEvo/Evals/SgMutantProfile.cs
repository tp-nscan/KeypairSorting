using System.Collections.Generic;
using System.Linq;
using MathUtils.Rand;
using SorterEvo.Genomes;
using SorterEvo.Layers;
using SorterEvo.Workflows;
using Sorting.CompetePools;

namespace SorterEvo.Evals
{

    public interface ISgMutantProfile
    {
        IReadOnlyList<ISorterGenomeEval> SorterGenomeEvals { get; }
        ISorterGenomeEval ParentGenomeEval { get; }
        IReadOnlyList<double> Scores { get; }
        ISorterMutateParams SorterMutateParams { get; }
    }

    public static class SgMutantProfile
    {
        public static ISgMutantProfile ToSgMutantProfile
            (
                this ISorterGenomeEval parentGenomeEval,
                ISorterMutateParams sorterMutateParams
            )
        {
            var rando = Rando.Fast(sorterMutateParams.Seed);

            var layer = SorterLayer.Make(new[] {parentGenomeEval.SorterGenome}, 0)
                                   .Reproduce(
                                        seed: rando.NextInt(),
                                        newGenomeCount: sorterMutateParams.MutantCount,
                                        mutationRate: sorterMutateParams.SorterMutationRate,
                                        insertionRate: sorterMutateParams.SorterInsertionRate,
                                        deletionRate: sorterMutateParams.SorterDeletionRate
                                   );

            var compPool = CompPool.MakeEmpty(parentGenomeEval.SorterGenome.KeyCount)
                                   .AddSorterEvalsParallel(layer.Genomes.Select(g => g.ToSorter()));

            return new SgMutantProfileImpl(
                    parentGenomeEval: parentGenomeEval,
                    sorterGenomeEvals: compPool.SorterEvals.Where(ev=>ev.SwitchUseCount <= sorterMutateParams.MaxScore)
                                               .Select(ev => SorterGenomeEval.Make
                                                    (
                                                        sorterGenome: layer.GetGenome(ev.Sorter.Guid),
                                                        parentGenomeEval: parentGenomeEval,
                                                        sorterEval: ev,
                                                        generation:1,
                                                        success: ev.Success
                                                    )),
                    scores: compPool.SorterEvals.Select(ev=>(double)ev.SwitchUseCount),
                    sorterMutateParams: sorterMutateParams
                );
        }
    }

    class SgMutantProfileImpl : ISgMutantProfile
    {
        private readonly IReadOnlyList<ISorterGenomeEval> _sorterGenomeEvals;
        private readonly ISorterGenomeEval _parentGenomeEval;
        private readonly IReadOnlyList<double> _scores;
        private readonly ISorterMutateParams _sorterMutateParams;

        public SgMutantProfileImpl(
            ISorterGenomeEval parentGenomeEval,
            IEnumerable<ISorterGenomeEval> sorterGenomeEvals, 
            IEnumerable<double> scores, 
            ISorterMutateParams sorterMutateParams
            )
        {
            _parentGenomeEval = parentGenomeEval;
            _sorterGenomeEvals = sorterGenomeEvals.ToList();
            _scores = scores.ToList();
            _sorterMutateParams = sorterMutateParams;
        }

        public IReadOnlyList<ISorterGenomeEval> SorterGenomeEvals
        {
            get { return _sorterGenomeEvals; }
        }

        public ISorterGenomeEval ParentGenomeEval
        {
            get { return _parentGenomeEval; }
        }

        public IReadOnlyList<double> Scores
        {
            get { return _scores; }
        }

        public ISorterMutateParams SorterMutateParams
        {
            get { return _sorterMutateParams; }
        }
    }
}
