﻿using System;
using System.Linq;
using Genomic.Genomes;
using Genomic.Layers;
using MathUtils.Rand;
using SorterEvo.Evals;
using SorterEvo.Genomes;
using SorterEvo.Layers;
using Sorting.CompetePools;

namespace SorterEvo.Workflows
{
    public interface ISorterCompPoolWorkflow
    {
        ICompPool CompPool { get; }
        int Generation { get; }
        ISorterCompPoolParams SorterCompPoolParams { get; }
        CompWorkflowState CompWorkflowState { get; }
        ISorterCompPoolWorkflow Step(int seed);
        ILayer<ISorterGenome> SorterLayer { get; }
        ILayerEval<ISorterGenome, IGenomeEval<ISorterGenome>> SorterLayerEval { get; }
    }

    public static class SorterCompPoolWorkflow
    {
        public static ISorterCompPoolWorkflow Make(
            int seed,
            int keyCount,
            int keyPairCount,
            ISorterCompPoolParams sorterCompPoolParams
        )
        {
            var randy = Rando.Fast(seed);
            return new SorterCompPoolWorkflowImpl
                (
                    sorterLayer: SorterLayer.Create
                                (
                                      seed: randy.NextInt(),
                                      genomeCount: sorterCompPoolParams.SorterLayerStartingGenomeCount,
                                      keyCount: keyCount,
                                      keyPairCount: keyPairCount
                                ),
                    sorterCompPoolParams: sorterCompPoolParams,
                    generation: 0
                );
        }

        public static ISorterCompPoolWorkflow Make(
            ILayer<ISorterGenome> sorterLayer,
            ISorterCompPoolParams sorterCompPoolParams,
            int generation
        )
        {
            return new SorterCompPoolWorkflowImpl
                (
                    sorterLayer: sorterLayer,
                    sorterCompPoolParams: sorterCompPoolParams,
                    generation: generation
                );
        }
    }

    class SorterCompPoolWorkflowImpl : ISorterCompPoolWorkflow
    {
        public SorterCompPoolWorkflowImpl(
            ILayer<ISorterGenome> sorterLayer,
            ISorterCompPoolParams sorterCompPoolParams, 
            int generation
            )
        {
            _sorterLayer = sorterLayer;
            _compWorkflowState = CompWorkflowState.ReproGenomes;
            _sorterCompPoolParams = sorterCompPoolParams;
            _generation = generation;
        }

        private SorterCompPoolWorkflowImpl
            (
                CompWorkflowState compWorkflowState,
                ILayer<ISorterGenome> sorterLayer,
                ICompPool compPool,
                ILayerEval<ISorterGenome, IGenomeEval<ISorterGenome>> sorterLayerEval,
                ISorterCompPoolParams sorterCompPoolParams, 
                int generation
            )
        {
            _compWorkflowState = compWorkflowState;
            _sorterLayer = sorterLayer;
            _sorterLayerEval = sorterLayerEval;
            _sorterCompPoolParams = sorterCompPoolParams;
            _generation = generation;
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

        private readonly ISorterCompPoolParams _sorterCompPoolParams;
        public ISorterCompPoolParams SorterCompPoolParams
        {
            get { return _sorterCompPoolParams; }
        }

        private readonly ILayer<ISorterGenome> _sorterLayer;
        public ILayer<ISorterGenome> SorterLayer
        {
            get { return _sorterLayer; }
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

        public ISorterCompPoolWorkflow Step(int seed)
        {
            ISorterCompPoolWorkflow sorterCompPoolWorkflow = null;
            switch (CompWorkflowState)
            {
                case CompWorkflowState.ReproGenomes:
                    sorterCompPoolWorkflow = ReproStep(seed);
                    break;
                case CompWorkflowState.RunCompetition:
                    sorterCompPoolWorkflow = RunCompetitionStep(seed);
                    break;
                case CompWorkflowState.EvaluateResults:
                    sorterCompPoolWorkflow = EvaluateResultsStep(seed);
                    break;
                case CompWorkflowState.UpdateGenomes:
                    sorterCompPoolWorkflow = UpdateGenomesStep(seed);
                    break;
                default:
                    throw new Exception(String.Format("CompWorkflowState {0} not handled in SorterCompParaPoolWorkflowImpl.Step", CompWorkflowState));
            }

            return sorterCompPoolWorkflow;
        }

        private ISorterCompPoolWorkflow ReproStep(int seed)
        {
            var randy = Rando.Fast(seed);

            var sorterLayer = SorterLayer.Reproduce
                (
                    seed: randy.NextInt(), 
                    newGenomeCount: SorterCompPoolParams.SorterLayerExpandedGenomeCount,
                    mutationRate: SorterCompPoolParams.SorterMutationRate,
                    insertionRate: SorterCompPoolParams.SorterMutationRate, 
                    deletionRate: SorterCompPoolParams.SorterDeletionRate
                );

            return new SorterCompPoolWorkflowImpl
                (
                    compWorkflowState: CompWorkflowState.RunCompetition,
                    sorterLayer: sorterLayer,
                    compPool: null,
                    sorterLayerEval: null,
                    sorterCompPoolParams: SorterCompPoolParams,
                    generation: Generation
                );

        }

        private ISorterCompPoolWorkflow RunCompetitionStep(int seed)
        {
            return new SorterCompPoolWorkflowImpl
                (
                    compWorkflowState: CompWorkflowState.EvaluateResults,
                    sorterLayer: SorterLayer,
                    compPool: SorterLayer.Genomes.Select(t => t.ToSorter()).ToCompPoolParallel(),
                    sorterLayerEval: null,
                    sorterCompPoolParams: SorterCompPoolParams,
                    generation: Generation
                );
        }

        private ISorterCompPoolWorkflow EvaluateResultsStep(int seed)
        {
            var sorterLayerEval =
                CompPool.SorterEvals.Select(
                    t => GenomeEval.Make(
                            genome: SorterLayer.GetGenome(t.Sorter.Guid),
                            score: t.SwitchUseCount,
                            generation: Generation
                        )
                    ).Make<ISorterGenome, IGenomeEval<ISorterGenome>>();

            return new SorterCompPoolWorkflowImpl
                (
                    compWorkflowState: CompWorkflowState.UpdateGenomes,
                    sorterLayer: SorterLayer,
                    compPool: CompPool,
                    sorterLayerEval: sorterLayerEval,
                    sorterCompPoolParams: SorterCompPoolParams,
                    generation: Generation
                );
        }

        private ISorterCompPoolWorkflow UpdateGenomesStep(int seed)
        {
            var rando = Rando.Fast(seed);
            return new SorterCompPoolWorkflowImpl
                (
                    compWorkflowState: CompWorkflowState.ReproGenomes,
                    sorterLayer: Layer.Make(
                        generation: Generation + 1,
                        genomes: SorterLayerEval.GenomeEvals
                                                .SubSortShuffle(t => t.Score, rando.NextInt())
                                                .Select(e => e.Genome)
                                                .Take(SorterCompPoolParams.SorterLayerStartingGenomeCount)

                                                //.Take(
                                                //    (Generation % 4 == 0) ?
                                                //        SorterCompPoolParams.SorterLayerStartingGenomeCount/4
                                                //    :
                                                //        SorterCompPoolParams.SorterLayerStartingGenomeCount
                                                // )
                        ),
                        compPool: null,
                        sorterLayerEval: null,
                        sorterCompPoolParams: SorterCompPoolParams,
                        generation: Generation + 1
                );
        }
    }
}
