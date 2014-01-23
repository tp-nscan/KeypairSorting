using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Entities.BackgroundWorkers;
using KeypairSorting.Resources;
using KeypairSorting.ViewModels.Parts;
using MathUtils.Rand;
using SorterEvo.Evals;
using SorterEvo.Layers;
using SorterEvo.Workflows;
using WpfUtils;

namespace KeypairSorting.ViewModels.MakeTunedSorters
{
    public class ScpRunnerVm : ViewModelBase
    {
        public ScpRunnerVm(IScpParams scpParams, IEnumerable<ISorterGenomeEvalVm> sorterGenomeEvalVms)
        {
            var genomeEvalVms = sorterGenomeEvalVms.ToList();

            _sorterGenomeEvalGridVmInitial = new SorterGenomeEvalGridVm("Progenitors");
            _sorterGenomeEvalGridVmInitial.SorterGenomeEvalVms.AddMany(genomeEvalVms);
            _sorterGenomeEvalGridVm = new SorterGenomeEvalGridVm("Current population");
            _sorterGenomeEvalGridVm.SorterGenomeEvalVms.AddMany(genomeEvalVms);
            _scpParamsVm = new ScpParamVm(scpParams);
        }

        private readonly Subject<Tuple<string, string>> _onIterationResult 
            = new Subject<Tuple<string, string>>();
        public IObservable<Tuple<string, string>> OnIterationResult
        {
            get { return _onIterationResult; }
        }

        public ConfigRunTemplateType ConfigRunTemplateType
        {
            get { return ConfigRunTemplateType.Run; }
        }

        private bool _busy;
        public bool Busy
        {
            get { return _busy; }
            private set
            {
                _busy = value;
                OnPropertyChanged("Busy");
            }
        }

        private readonly SorterGenomeEvalGridVm _sorterGenomeEvalGridVmInitial;
        public SorterGenomeEvalGridVm SorterGenomeEvalGridVmInitial
        {
            get { return _sorterGenomeEvalGridVmInitial; }
        }

        private readonly SorterGenomeEvalGridVm _sorterGenomeEvalGridVm;
        public SorterGenomeEvalGridVm SorterGenomeEvalGridVm
        {
            get { return _sorterGenomeEvalGridVm; }
        }

        private readonly ScpParamVm _scpParamsVm;
        public ScpParamVm ScpParamsVm
        {
            get { return _scpParamsVm; }
        }


        #region RunCommand

        private CancellationTokenSource _cancellationTokenSource;
        public async Task OnRunAsync(CancellationTokenSource cancellationTokenSource)
        {
            Busy = true;
            _cancellationTokenSource = cancellationTokenSource;

            var rando = Rando.Fast(ScpParamsVm.Seed);

            var rbw = RecursiveBackgroundWorker.Make
                (
                     initialState: ScpWorkflow.Make
                        (
                           sorterLayer: SorterLayer.Make(
                               sorterGenomes: _sorterGenomeEvalGridVm.SorterGenomeEvalVms
                                                                            .Select(t => t.GetSorterGenomeEval().SorterGenome),
                               generation: ScpParamsVm.CurrentGeneration
                           ),
                           scpParams: ScpParamsVm.GetParams,
                           generation: ScpParamsVm.CurrentGeneration
                        ),

                   recursion: (i, c) =>
                   {
                       var nextStep = i;
                       while (true)
                       {
                           if (_cancellationTokenSource.IsCancellationRequested)
                           {
                               return IterationResult.Make<IScpWorkflow>(null, ProgressStatus.StepIncomplete);
                           }

                           nextStep = nextStep.Step(rando.NextInt());

                           if (nextStep.CompWorkflowState == CompWorkflowState.ReproGenomes)
                           {
                               return IterationResult.Make(nextStep, ProgressStatus.StepComplete);
                           }
                       }
                   },
                   totalIterations: ScpParamsVm.TotalGenerations - ScpParamsVm.CurrentGeneration,
                   cancellationTokenSource: _cancellationTokenSource
                );

            rbw.OnIterationResult.Subscribe(UpdateSorterTuneResults);
            await rbw.Start();

            Busy = false;
        }

        readonly Dictionary<Guid, ISorterGenomeEval> _sorterGenomeEvals = new Dictionary<Guid, ISorterGenomeEval>();

        private void UpdateSorterTuneResults(IIterationResult<IScpWorkflow> result)
        {
            if (
                    (result.ProgressStatus == ProgressStatus.StepComplete) 
                    && 
                    (result.Data.SorterLayerEval != null)
               )
            {
                ScpParamsVm.CurrentGeneration = result.Data.Generation;
                SorterGenomeEvalGridVm.SorterGenomeEvalVms.Clear();

                var sorterEvalDict = result.Data.CompPool.SorterEvals.ToDictionary(e => e.Sorter.Guid);


                var currentEvals =
                    result.Data.SorterLayerEval.GenomeEvals
                        .Select(e =>
                                    SorterGenomeEval.Make
                                    (
                                        sorterGenome: e.Genome,
                                        ancestors: ImmutableStack<int>.Empty.Push((int)e.Score),
                                        sorterEval: sorterEvalDict[e.Guid],
                                        generation: ScpParamsVm.CurrentGeneration
                                    )
                               ).ToList();


                SorterGenomeEvalGridVm
                        .SorterGenomeEvalVms
                        .AddMany(
                                  currentEvals.OrderBy(e => e.Score)
                                              .Take(100)
                                              .Select(g=>g.ToSorterGenomeEvalVm())
                               );


                var groips = sorterEvalDict.Values
                                        .GroupBy(e => e.SwitchUseCount)
                                        .OrderBy(g=>g.Key)
                                        .Select(g => "[" + g.Key + "," + g.Count() + "]\t")
                                        .Aggregate(string.Empty, (e, n) => e + n);

                _onIterationResult.OnNext(new Tuple<string, string>(ScpParamsVm.Name, groips));
            }
        }


        //readonly Dictionary<Guid, ISorterGenomeEval> _sorterGenomeEvals = new Dictionary<Guid, ISorterGenomeEval>();

        //private void UpdateSorterTuneResults(IIterationResult<IScpWorkflow> result)
        //{
        //    if (result.ProgressStatus == ProgressStatus.StepComplete)
        //    {
        //        ScpParamsVm.CurrentGeneration = result.Data.Generation;
        //        SorterGenomeEvalGridVm.SorterGenomeEvalVms.Clear();

        //        var sorterEvalDict = result.Data.CompPool.SorterEvals.ToDictionary(e => e.Sorter.Guid);

        //        foreach (var sorterGenomeEval in result.Data.SorterLayerEval.GenomeEvals)
        //        {
        //            if (_sorterGenomeEvals.ContainsKey(sorterGenomeEval.Genome.ParentGuid))
        //            {
        //                var oldSorterEval = _sorterGenomeEvals[sorterGenomeEval.Genome.ParentGuid];
        //                _sorterGenomeEvals[sorterGenomeEval.Guid] = SorterGenomeEval.Make
        //                    (
        //                        sorterGenome: sorterGenomeEval.Genome,
        //                        parentGenomeEval: oldSorterEval,
        //                        sorterEval: sorterEvalDict[sorterGenomeEval.Guid],
        //                        generation: ScpParamsVm.CurrentGeneration
        //                   );
        //            }
        //            else
        //            {
        //                _sorterGenomeEvals[sorterGenomeEval.Guid] = SorterGenomeEval.Make
        //                    (
        //                        sorterGenome: sorterGenomeEval.Genome,
        //                        ancestors: ImmutableStack<Guid>.Empty.Push(sorterGenomeEval.Genome.ParentGuid),
        //                        sorterEval: sorterEvalDict[sorterGenomeEval.Guid],
        //                        generation: ScpParamsVm.CurrentGeneration
        //                   );
        //            }
        //        }

        //        var evalGuidList = _sorterGenomeEvals.Select(t => t.Key).ToList();
        //        foreach (var evalGuid in evalGuidList)
        //        {
        //            if (_sorterGenomeEvals[evalGuid].Generation < ScpParamsVm.CurrentGeneration - 1)
        //            {
        //                _sorterGenomeEvals.Remove(evalGuid);
        //            }
        //        }

        //        var orderedEvals = _sorterGenomeEvals.Values
        //            .OrderBy(r => r.SorterEval.SwitchUseCount)
        //                            .Take(50)
        //                            .ToList();
        //        foreach (var sorterGenomeEval in orderedEvals)
        //        {
        //            SorterGenomeEvalGridVm.SorterGenomeEvalVms.Add(sorterGenomeEval.ToSorterGenomeEvalVm());
        //        }

        //        _onIterationResult.OnNext(new Tuple<string, ISorterGenomeEval>(ScpParamsVm.Name, orderedEvals.First()));
        //    }
        //}

        public bool CanRun()
        {
            return !_busy;
        }

        #endregion // RunCommand

    }
}
