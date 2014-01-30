using System;
using System.Collections.Generic;
using System.Linq;
using Genomic.Genomes;
using Genomic.Layers;
using MathUtils.Collections;
using MathUtils.Rand;
using Newtonsoft.Json;
using SorterEvo.Evals;
using SorterEvo.Genomes;
using SorterEvo.Layers;
using Sorting.CompetePools;
using Sorting.Sorters;


namespace SorterEvo.Workflows
{
    public interface IScpWorkflow
    {
        ICompPool CompPool { get; }
        int Generation { get; }
        IScpParams ScpParams { get; }
        CompWorkflowState CompWorkflowState { get; }
        IScpWorkflow Step(int seed);
        string Report { get; }
        ILayerEval<ISorterGenome, IGenomeEval<ISorterGenome>> SorterLayerEval { get; }
    }

    public static class ScpWorkflow
    {
        public static IScpWorkflow Make(
            int seed,
            int keyCount,
            int keyPairCount,
            IScpParams scpParams
        )
        {
            var randy = Rando.Fast(seed);
            return new ScpWorkflowImpl
                (
                    sorterLayer0: null,
                    sorterLayer1: SorterLayer.Create
                                (
                                      seed: randy.NextInt(),
                                      genomeCount: scpParams.PopulationCount,
                                      keyCount: keyCount,
                                      keyPairCount: keyPairCount
                                ),
                    scpParams: scpParams,
                    generation: 0,
                    report: string.Empty
                );
        }

        public static IScpWorkflow Make(
            ILayer<ISorterGenome> sorterLayer,
            IScpParams scpParams,
            int generation
        )
        {
            return new ScpWorkflowImpl
                (
                    sorterLayer0: Layer.Make(
                            generation: generation,
                            genomes: Enumerable.Empty<ISorterGenome>()
                        ),
                    sorterLayer1: sorterLayer,
                    scpParams: scpParams,
                    generation: generation,
                    report: string.Empty
                );
        }
    }

    class ScpWorkflowImpl : IScpWorkflow
    {
        public ScpWorkflowImpl
            (
                ILayer<ISorterGenome> sorterLayer0, 
                ILayer<ISorterGenome> sorterLayer1,
                IScpParams scpParams,
                int generation, 
                string report
            )
        {
            _sorterLayer0 = sorterLayer0;
            _compWorkflowState = CompWorkflowState.RunCompetition;
            _scpParams = scpParams;
            _generation = generation;
            _report = report;
            _sorterLayer1 = sorterLayer1;
            _sorterLayerEval = Enumerable.Empty<ISorterGenomeEval>()
                                         .Make<ISorterGenome, IGenomeEval<ISorterGenome>>();
            _compPool = Sorting.CompetePools.CompPool.MakeEmpty(_sorterLayer1.Genomes.First().KeyCount);
        }

        private ScpWorkflowImpl
            (
                CompWorkflowState compWorkflowState,
                ILayer<ISorterGenome> sorterLayer0, 
                ILayer<ISorterGenome> sorterLayer1,
                ICompPool compPool,
                ILayerEval<ISorterGenome, IGenomeEval<ISorterGenome>> sorterLayerEval,
                IScpParams scpParams,
                int generation, 
                string report
            )
        {
            _compWorkflowState = compWorkflowState;
            _sorterLayer0 = sorterLayer0;
            _sorterLayerEval = sorterLayerEval;
            _scpParams = scpParams;
            _generation = generation;
            _report = report;
            _sorterLayer1 = sorterLayer1;
            _compPool = compPool;
        }

        private readonly ICompPool _compPool;
        public ICompPool CompPool
        {
            get { return _compPool; }
        }


        private readonly int _generation;
        public int Generation
        {
            get { return _generation; }
        }

        private readonly IScpParams _scpParams;
        public IScpParams ScpParams
        {
            get { return _scpParams; }
        }

        private readonly ILayer<ISorterGenome> _sorterLayer0;
        public ILayer<ISorterGenome> SorterLayer0
        {
            get { return _sorterLayer0; }
        }

        private readonly ILayer<ISorterGenome> _sorterLayer1;
        public ILayer<ISorterGenome> SorterLayer1
        {
            get { return _sorterLayer1; }
        }

        private readonly ILayerEval<ISorterGenome, IGenomeEval<ISorterGenome>> _sorterLayerEval;
        public ILayerEval<ISorterGenome, IGenomeEval<ISorterGenome>> SorterLayerEval
        {
            get { return _sorterLayerEval; }
        }

        private readonly CompWorkflowState _compWorkflowState;
        public CompWorkflowState CompWorkflowState
        {
            get { return _compWorkflowState; }
        }

        public IScpWorkflow Step(int seed)
        {
            IScpWorkflow scpWorkflow = null;
            switch (CompWorkflowState)
            {
                case CompWorkflowState.ReproGenomes:
                    scpWorkflow = ReproStep(seed);
                    break;
                case CompWorkflowState.RunCompetition:
                    scpWorkflow = RunCompetitionStep(seed);
                    break;
                case CompWorkflowState.EvaluateResults:
                    scpWorkflow = EvaluateResultsStep(seed);
                    break;
                case CompWorkflowState.UpdateGenomes:
                    scpWorkflow = UpdateGenomesStepH(seed);
                    break;
                default:
                    throw new Exception(String.Format("CompWorkflowState {0} not handled in SorterCompParaPoolWorkflowImpl.Step", CompWorkflowState));
            }

            return scpWorkflow;
        }

        private readonly string _report;
        public string Report
        {
            get { return _report; }
        }

        private IScpWorkflow ReproStep(int seed)
        {
            var randy = Rando.Fast(seed);

            var sorterLayer1 = SorterLayer0.Reproduce
                (
                    seed: randy.NextInt(),
                    newGenomeCount: ScpParams.ChildCount,
                    mutationRate: ScpParams.SorterMutationRate,
                    insertionRate: ScpParams.SorterMutationRate,
                    deletionRate: ScpParams.SorterDeletionRate
                );

            //sorterLayer1 = sorterLayer1.Recombinate(ScpParams.SorterRecombinationRate, randy.NextInt());

            return new ScpWorkflowImpl
                (
                    compWorkflowState: CompWorkflowState.RunCompetition,
                    sorterLayer0: _sorterLayer0,
                    sorterLayer1: sorterLayer1,
                    compPool: CompPool,
                    sorterLayerEval: SorterLayerEval,
                    scpParams: ScpParams,
                    generation: Generation,
                    report: string.Empty
                );
        }

        private IScpWorkflow RunCompetitionStep(int seed)
        {
            var newGenomes = SorterLayer1.Genomes
                .Where(g => SorterLayerEval.GetGenomeEval(g.Guid) == null);

            return new ScpWorkflowImpl
                (
                    compWorkflowState: CompWorkflowState.EvaluateResults,
                    sorterLayer0: SorterLayer0,
                    sorterLayer1: _sorterLayer1,
                    compPool: CompPool.AddSorterEvalsParallel(newGenomes.Select(t => t.ToSorter())),
                    sorterLayerEval: SorterLayerEval,
                    scpParams: ScpParams,
                    generation: Generation,
                    report: string.Empty
                );
        }

        private IScpWorkflow EvaluateResultsStep(int seed)
        {
            var sorterLayerEval =
                    CompPool.SorterEvals
                    .Where(t=>SorterLayer1.GetGenome(t.Sorter.Guid) != null)
                    .Select(
                        t => GenomeEval.Make(
                                genome: SorterLayer1.GetGenome(t.Sorter.Guid),
                                score: t.SwitchUseCount,
                                generation: Generation
                            )
                        ).Concat(SorterLayerEval.GenomeEvals)
                         .Make<ISorterGenome, IGenomeEval<ISorterGenome>>();

            return new ScpWorkflowImpl
                (
                    compWorkflowState: CompWorkflowState.UpdateGenomes,
                    sorterLayer0: SorterLayer0,
                    sorterLayer1: _sorterLayer1,
                    compPool: CompPool,
                    sorterLayerEval: sorterLayerEval,
                    scpParams: ScpParams,
                    generation: Generation,
                    report: string.Empty
                );
        }

        private IScpWorkflow UpdateGenomesStep(int seed)
        {
            var evaluatedGenomes = SorterLayerEval.GenomeEvals
                                            .SubSortShuffle(e=>e.Score, seed)
                                            .ToList();
            var legacyCount = 0;
            var cubCount = 0;
            var leftoverCount = 0;
            var leftoverTotal = ScpParams.PopulationCount - ScpParams.CubCount - ScpParams.LegacyCount;
            var newGenomes = new List<ISorterGenome>();


            foreach (var evaluatedGenome in evaluatedGenomes)
            {
                if (
                        SorterLayer0.GetGenome(evaluatedGenome.Guid) != null
                        &&
                        legacyCount < ScpParams.LegacyCount
                   )
                {
                    newGenomes.Add(evaluatedGenome.Genome);
                    legacyCount++;
                }

                else if (
                        SorterLayer1.GetGenome(evaluatedGenome.Guid) != null
                        &&
                        cubCount < ScpParams.CubCount
                   )
                {
                    newGenomes.Add(evaluatedGenome.Genome);
                    cubCount++;
                }
                else if(leftoverCount < leftoverTotal)
                {
                    newGenomes.Add(evaluatedGenome.Genome);
                    leftoverCount++;

                    if (SorterLayer0.GetGenome(evaluatedGenome.Guid) != null)
                    {
                        legacyCount++;
                    }
                    else
                    {
                        cubCount++;
                    }
                }
            }

            return new ScpWorkflowImpl
                (
                    compWorkflowState: CompWorkflowState.ReproGenomes,
                    sorterLayer0: Layer.Make(
                            generation: Generation + 1,
                            genomes: newGenomes
                        ),
                    sorterLayer1: null,
                    compPool: CompPool.TrimEvalsToTheseIds(newGenomes.Select(g=>g.Guid)),
                    sorterLayerEval: newGenomes.Select(g => SorterLayerEval.GetGenomeEval(g.Guid))
                                               .Make<ISorterGenome, IGenomeEval<ISorterGenome>>(),
                    scpParams: ScpParams,
                    generation: Generation + 1,
                    report: JsonConvert.SerializeObject(new Tuple<int, int>(legacyCount, cubCount),Formatting.None)
                );
        }


        private IScpWorkflow UpdateGenomesStepH(int seed)
        {
            var evaluatedGenomes = SorterLayerEval.GenomeEvals
                                            .SubSortShuffle(e => e.Score, seed)
                                            .ToList();
            var legacyCount = 0;
            var cubCount = 0;
            var leftoverCount = 0;
            var leftoverTotal = ScpParams.PopulationCount - ScpParams.CubCount - ScpParams.LegacyCount;
            //var newGenomes = new List<ISorterGenome>();
            var newGenomes = new Dictionary<string, ISorterGenome>();

          

            foreach (var evaluatedGenome in evaluatedGenomes)
            {
                var sorterStages = CompPool.SorterEval(evaluatedGenome.Guid)
                    .Reduce(Guid.NewGuid()).ToStagedSorter()
                    .SorterStages
                    .ToList();

                var hash = evaluatedGenome.Score + " " +
                sorterStages.Repeat().Skip((Generation / 5) % sorterStages.Count).Take(5)
                    .Select(s=>s.KeyPairs.Select(kp=>kp.Index)
                                         .Aggregate(string.Empty, (str,i) => str + "," + i)
                           )
                    .Aggregate(string.Empty, (st,i) => st + (String.IsNullOrEmpty(st) ? "" : ",") + i);
                    

                if (newGenomes.ContainsKey(hash))
                {
                    continue;
                }

                if (
                        SorterLayer0.GetGenome(evaluatedGenome.Guid) != null
                        &&
                        legacyCount < ScpParams.LegacyCount
                   )
                {
                    newGenomes[hash] = evaluatedGenome.Genome;
                    legacyCount++;
                }

                else if (
                        SorterLayer1.GetGenome(evaluatedGenome.Guid) != null
                        &&
                        cubCount < ScpParams.CubCount
                   )
                {
                    newGenomes[hash] = evaluatedGenome.Genome;
                    cubCount++;
                }
                else if (leftoverCount < leftoverTotal)
                {
                    newGenomes[hash] = evaluatedGenome.Genome;
                    leftoverCount++;

                    if (SorterLayer0.GetGenome(evaluatedGenome.Guid) != null)
                    {
                        legacyCount++;
                    }
                    else
                    {
                        cubCount++;
                    }
                }
            }

            return new ScpWorkflowImpl
                (
                    compWorkflowState: CompWorkflowState.ReproGenomes,
                    sorterLayer0: Layer.Make(
                            generation: Generation + 1,
                            genomes: newGenomes.Values
                        ),
                    sorterLayer1: null,
                    compPool: CompPool.TrimEvalsToTheseIds(newGenomes.Select(g => g.Value.Guid)),
                    sorterLayerEval: newGenomes.Select(g => SorterLayerEval.GetGenomeEval(g.Value.Guid))
                                               .Make<ISorterGenome, IGenomeEval<ISorterGenome>>(),
                    scpParams: ScpParams,
                    generation: Generation + 1,
                    report: JsonConvert.SerializeObject(new Tuple<int, int>(legacyCount, cubCount), Formatting.None)
                );
        }
    }
}
