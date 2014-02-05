using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Entities.BackgroundWorkers;
using KeypairSorting.ViewModels.Parts;
using MathUtils.Rand;
using Newtonsoft.Json;
using SorterEvo.Evals;
using SorterEvo.Layers;
using SorterEvo.Workflows;
using WpfUtils;

namespace KeypairSorting.ViewModels.Scp
{
    public class ScpRunnerVm : ViewModelBase
    {
        public ScpRunnerVm(IScpParams scpParams, IEnumerable<ISorterGenomeEvalVm> sorterGenomeEvalVms)
        {
            var genomeEvalVms = sorterGenomeEvalVms.ToList();

            _sorterGenomeEvalGridVmInitial = new SorterGenomeEvalGridVm("Progenitors");
            _sorterGenomeEvalGridVmInitial.SorterGenomeEvalVms.AddMany(genomeEvalVms);
            _sorterGenomeEvalGridVm = new SorterGenomeEvalGridVm("Current population");

            _sorterGenomeEvals = genomeEvalVms.Select(v => v.SorterGenomeEval).ToDictionary(e => e.Guid);
            _scpParamsVm = new ScpParamVm(scpParams);
        }

        Dictionary<Guid, ISorterGenomeEval> _sorterGenomeEvals = new Dictionary<Guid, ISorterGenomeEval>();

        private readonly Subject<Tuple<string, int, string>> _onIterationResult 
            = new Subject<Tuple<string, int, string>>();
        public IObservable<Tuple<string, int, string>> OnIterationResult
        {
            get { return _onIterationResult; }
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

        #region Run functions

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
                               sorterGenomes: _sorterGenomeEvals.Select(e=>e.Value.SorterGenome),
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

        private void UpdateSorterTuneResults(IIterationResult<IScpWorkflow> result)
        {
            if (
                    (result.ProgressStatus == ProgressStatus.StepComplete) 
                    && 
                    (result.Data.SorterLayerEval != null)
               )
            {
                var sorterEvalDict = result.Data.CompPool.SorterEvals.ToDictionary(e => e.Sorter.Guid);

                var nextEvals = new List<ISorterGenomeEval>();
                foreach (var genomeEval in result.Data.SorterLayerEval.GenomeEvals.Where(ev=>ev.Success))
                {
                    if (_sorterGenomeEvals.ContainsKey(genomeEval.Guid))
                    {
                        nextEvals.Add(_sorterGenomeEvals[genomeEval.Guid]);
                    }
                    else
                    {
                        nextEvals.Add
                            (
                                    SorterGenomeEval.Make
                                    (
                                        sorterGenome: genomeEval.Genome,
                                        parentGenomeEval: _sorterGenomeEvals.ContainsKey(genomeEval.Genome.ParentGuid) 
                                                           ?  _sorterGenomeEvals[genomeEval.Genome.ParentGuid] : null,
                                        sorterEval: sorterEvalDict[genomeEval.Guid],
                                        generation: ScpParamsVm.CurrentGeneration,
                                        success: genomeEval.Success
                                    )
                            );
                    }
                }

                ScpParamsVm.CurrentGeneration = result.Data.Generation;

                if (ScpParamsVm.CurrentGeneration % ReportFrequency == 0)
                {
                    SorterGenomeEvalGridVm.SorterGenomeEvalVms.Clear();

                    SorterGenomeEvalGridVm
                            .SorterGenomeEvalVms
                            .AddMany(
                                      nextEvals.OrderBy(e => e.Score)
                                                  .ThenByDescending(e => e.Generation)
                                                  .Take(1000)
                                                  .Select(g => g.ToSorterGenomeEvalVm())
                                    );

                }

                //var groips = result.Data.SorterLayerEval.GenomeEvals
                //                        .GroupBy(e => e.Score)
                //                        .OrderBy(g => g.Key)
                //                        .Select(g =>  string.Format("[{0}, {1}]", g.Key, g.Count()).PadLeft(12))
                //                        .Aggregate(string.Empty, (e, n) => e + n);

                var groips = JsonConvert.SerializeObject
                    ( 
                             result.Data.SorterLayerEval.GenomeEvals
                                        .GroupBy(e => e.Score)
                                        .OrderBy(g => g.Key)
                                        .Select(g => new Tuple<int, int>((int) g.Key, g.Count()))
                                        .ToList()
                    );

                _sorterGenomeEvals = nextEvals.ToDictionary(e => e.Guid);
                if (ScpParamsVm.CurrentGeneration % ReportFrequency == 0)
                {
                    _onIterationResult.OnNext(new Tuple<string, int, string>(ScpParamsVm.Name, ScpParamsVm.CurrentGeneration, groips));
                }
            }
        }

        #endregion // Run functions

        public int? ReportFrequency { get; set; }

    }
}
